using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Types;

namespace GammaDraconis.Video
{
    /// <summary>
    /// Properties that can be set on any object added to the scene manager.
    /// eg: Scene.track(object, GO_TYPE.RACER | GO_TYPE.GHOST) 
    /// </summary>
    public static class GO_TYPE
    {
        static public int SKYBOX = 1;
        static public int SCENERY = 2; // Never checked for thinking or physics, always drawn first and facing the viewer, uncollideable
        static public int HUD = 4;
        static public int THINKABLE = 8;
        static public int MOVABLE = 16;
        static public int COLLIDABLE = 32;

        // Composite
        static public int GHOST = THINKABLE | MOVABLE;
        static public int NORMAL = THINKABLE | MOVABLE | COLLIDABLE;
        static public int DEBRIS = MOVABLE | COLLIDABLE;

        static public int RACER = NORMAL;
        static public int BULLET = NORMAL;
        static public int MISSILE = NORMAL;
        static public int COURSE = GHOST;
    }
    
    /// <summary>
    /// The scene manager holds the 'world' the game is contained within.
    /// Background scenery, game objects, and other such items should be kept here.
    /// </summary>
    class Scene
    {
        
        // References to all objects in the scene, *including* the player objects
        private Dictionary<int, List<GameObject>> objects;
        private OctreeLeaf octTreeRoot;
        public bool debugBoundingBoxSize = false;

        /// <summary>
        /// Create a new Scene manager.
        /// </summary>
        public Scene()
        {
            objects = new Dictionary<int, List<GameObject>>();
            //TODO: find root bounding box size
            octTreeRoot = new OctreeLeaf(new BoundingBox(new Vector3(-7500f),new Vector3(7500f)), 3, 0);
            octTreeRoot.setContainedObjects(new List<GameObject>());
        }

        /// <summary>
        /// Add an existing item to the scene manager with a given set of options.
        /// </summary>
        /// <param name="gameObject">Item to be tracked</param>
        /// <param name="type">Item properties</param>
        public void track(GameObject gameObject, int type)
        {
            if (objects.ContainsKey(type))
            {
                ((List<GameObject>)objects[type]).Add(gameObject);
            }
            else
            {
                List<GameObject> temp = new List<GameObject>();
                temp.Add(gameObject);
                objects.Add(type, temp);
            }
        }

        /// <summary>
        /// Remove an existing item from the scene manager.
        /// </summary>
        /// <param name="gameObject">Item to be removed</param>
        public void ignore(GameObject gameObject, int type)
        {
            if (objects.ContainsKey(type))
            {
                ((List<GameObject>)objects[type]).Remove(gameObject);
            }
        }

        /// <summary>
        /// Remove an existing item from the scene manager - use ignore (gameObject, type) if type is known
        /// </summary>
        /// <param name="gameObject">Item to be removed</param>
        public void ignore(GameObject gameObject)
        {
            foreach (List<GameObject> objs in objects.Values)
            {
                objs.Remove(gameObject);
            }
        }

        /// <summary>
        /// Return a list of GameObjects that are collidable.
        /// </summary>
        /// <returns>GameObjects to check for collision</returns>
        public List<GameObject> collidable()
        {
            List<GameObject> collidables = typedObjects(GO_TYPE.COLLIDABLE);
            return collidables;
        }

        /// <summary>
        /// Return a list of GameObjects that can move (not scenery or HUD objects)
        /// </summary>
        /// <returns>GameObjects to check for movement</returns>
        public List<GameObject> movable()
        {
            List<GameObject> movables = typedObjects(GO_TYPE.MOVABLE);
            return movables;
        }

        /// <summary>
        /// Return a list of GameObjects that should have think() called.
        /// </summary>
        /// <returns>GameObjects to think()</returns>
        public List<GameObject> thinkable()
        {
            List<GameObject> thinkables = typedObjects(GO_TYPE.THINKABLE);
            return thinkables;
        }

        /// <summary>
        /// Return a list of GameObjects that are within range and 
        /// viewing arc of the given vantage point coordinates.
        /// </summary>
        /// <param name="vantage">Vantage point Coords to render from</param>
        /// <returns>List of GameObjects to render</returns>
        public List<GameObject> visible(Coords vantage)
        {

            updateOctTreeObjects();

            List<GameObject> visibleObjects;
            Dictionary<int, List<GameObject>> optimizedObjects = sortOctTree(out visibleObjects, vantage);
            List<GameObject> temp = new List<GameObject>();
            List<GameObject> tempScenery = new List<GameObject>();
            List<GameObject> tempSkybox = new List<GameObject>();
            float aspRatio = GammaDraconis.renderer.aspectRatio;
            float viewAngle = GammaDraconis.renderer.viewingAngle;
            float viewDist = GammaDraconis.renderer.viewingDistance;

            Matrix view = Matrix.CreateLookAt(vantage.pos() - Matrix.CreateFromQuaternion(vantage.R).Forward, vantage.pos(), Matrix.CreateFromQuaternion(vantage.R).Up);
            BoundingFrustum viewFrustum = new BoundingFrustum(view * Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(viewAngle), aspRatio, 0.1f, viewDist));
            
            foreach (int tempKey in optimizedObjects.Keys)
            {
                List<GameObject> atemp = (List<GameObject>)optimizedObjects[tempKey];
                foreach (GameObject gameobject in atemp)
                {
                    // Take care of some quick cases before doing any math.
                    if ((tempKey & GO_TYPE.SKYBOX) == GO_TYPE.SKYBOX)
                    {
                        tempSkybox.Add(gameobject);
                        gameobject.position.T = Matrix.CreateTranslation(vantage.pos());
                    }
                    else
                    {
                        
                        if (viewFrustum.Contains(new BoundingSphere(gameobject.position.pos(), gameobject.size)) != ContainmentType.Disjoint)
                        {
                            if ((tempKey & GO_TYPE.SCENERY) == GO_TYPE.SCENERY)
                            {
                                tempScenery.Add(gameobject);
                                gameobject.position.R = Quaternion.CreateFromRotationMatrix(Matrix.CreateBillboard(vantage.pos(), gameobject.position.pos(), Vector3.One, Vector3.Forward));
                            }
                            else
                            {
                                temp.Add(gameobject);
                            }
                        }
                    }
                }
            }

            visibleObjects.AddRange(tempSkybox);
            visibleObjects.AddRange(tempScenery);
            visibleObjects.AddRange(temp);
            return visibleObjects;
        }

        /// <summary>
        /// Return a list of GameObjects that match a specialized type
        /// or a generic type.
        /// </summary>
        /// <param name="ofType">Type which must be matched</param>
        /// <returns>List of matching GameObjects</returns>
        public List<GameObject> typedObjects(int ofType)
        {
            List<GameObject> tObjects = new List<GameObject>();
            foreach (int tempKey in objects.Keys)
            {
                if ((tempKey & ofType) != 0)
                {
                    List<GameObject> temp = (List<GameObject>)objects[tempKey];
                    foreach(GameObject gameobject in temp)
                    {
                        tObjects.Add(gameobject);
                    }
                }
            }
            return tObjects;
        }

        public Dictionary<int, List<GameObject>> sortOctTree(out List<GameObject> entirelyVisible, Coords vantage)
        {
            entirelyVisible = new List<GameObject>();
            //TODO: Look through the oct tree and return a more optimized Hashtable and seed entirely Visible with objects that are obviously visible
            return objects;
        }
    
        public void updateOctTreeObjects()
        {
            List<GameObject> objList = new List<GameObject>();
            foreach (List<GameObject> tempList in objects.Values)
            {
                objList.AddRange(tempList);
            }
            octTreeRoot.setContainedObjects(objList);
            List<GameObject> outsideObjects = octTreeRoot.outsideOctree(objList);

            String output = "----------------------------------------------------------------------------------------------\n";
            if (outsideObjects.Count == 0)
            {
                output += "All objects contained within octree.\n";
            }
            else
            {
                foreach (GameObject outsideObj in outsideObjects)
                {
                    output += "GameObject: " + outsideObj + " at position: " + outsideObj.position.pos() + " not contained within the OcTree.\n";
                }
            }
            if (debugBoundingBoxSize)
            {
                Console.WriteLine(output);
            }
           
        }
    }
}
