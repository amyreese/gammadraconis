using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Types;
using GammaDraconis.Core;


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
        static public int DUST = 4;
        static public int CHECKPOINT = 8;
        static public int THINKABLE = 16;
        static public int MOVABLE = 32;
        static public int COLLIDABLE = 64;
        static public int INPUT_BASED = 128;
        static public int DIRECTIONAL_ARROW = 256;

        // Composite
        static public int GHOST = THINKABLE | MOVABLE;
        static public int NORMAL = THINKABLE | MOVABLE | COLLIDABLE;
        static public int DEBRIS = MOVABLE | COLLIDABLE;

        static public int RACER = NORMAL;
        static public int PLAYER = RACER | INPUT_BASED;
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
        public List<Room> rooms;
        private bool disableOctTree = false;

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
            rooms = new List<Room>();
            //TODO: find root bounding box size
            octTreeRoot = new OctreeLeaf(new BoundingBox(new Vector3(-15000f), new Vector3(15000f)), 3, 0);
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
                if (!objects[type].Contains(gameObject))
                {
                    objects[type].Add(gameObject);
                }
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
                objects[type].Remove(gameObject);
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

        public List<GameObject> visible(Player player)
        {
            return visible(player.getCamera(), player);
        }

        /// <summary>
        /// Return a list of GameObjects that are within range and 
        /// viewing arc of the given vantage point coordinates.
        /// </summary>
        /// <param name="vantage">Vantage point Coords to render from</param>
        /// <param name="player">player relevant to the scene</param>
        /// <returns>List of GameObjects to render</returns>
      
        
        public List<GameObject> visible(Coords vantage, Player player)
        {
            Room roomIn = null;
            foreach (Room room in rooms)
            {
                if (room.area.Contains(vantage.pos()) != ContainmentType.Disjoint)
                {
                    roomIn = room;
                    break;
                }
            }
            Dictionary<int, List<GameObject>> visibleObjects = new Dictionary<int, List<GameObject>>();
                
            if (roomIn == null || roomIn.canSeeOutside)
            {
                updateOctTreeObjects();


                float aspRatio = GammaDraconis.renderer.aspectRatio;
                float viewAngle = GammaDraconis.renderer.viewingAngle;
                float viewDist = GammaDraconis.renderer.viewingDistance;

                Matrix view = Matrix.CreateLookAt(vantage.pos() - Matrix.CreateFromQuaternion(vantage.R).Forward, vantage.pos(), Matrix.CreateFromQuaternion(vantage.R).Up);
                BoundingFrustum viewFrustum = new BoundingFrustum(view * Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(viewAngle), aspRatio, 0.1f, viewDist));

                Dictionary<int, List<GameObject>> optimizedObjects = sortOctTree(out visibleObjects, viewFrustum);
              

                foreach (int tempKey in optimizedObjects.Keys)
                {
                    if (!visibleObjects.ContainsKey(tempKey))
                        visibleObjects.Add(tempKey, new List<GameObject>());

                    List<GameObject> tempKeyObjects = optimizedObjects[tempKey];
                    foreach (GameObject gameobject in tempKeyObjects)
                    {
                        // Take care of some quick cases before doing any math.
                        if ((tempKey & GO_TYPE.SKYBOX) == GO_TYPE.SKYBOX)
                        {
                            visibleObjects[GO_TYPE.SKYBOX].Add(gameobject);
                            gameobject.position.T = Matrix.CreateTranslation(vantage.pos());
                        }
                        else
                        {

                            if (viewFrustum.Contains(new BoundingSphere(gameobject.position.pos(), gameobject.size)) != ContainmentType.Disjoint)
                            {
                                if ((tempKey & GO_TYPE.SCENERY) == GO_TYPE.SCENERY)
                                {
                                    gameobject.position.R = Quaternion.CreateFromRotationMatrix(Matrix.CreateBillboard(vantage.pos(), gameobject.position.pos(), Vector3.One, Vector3.Forward));
                                }
                                
                                visibleObjects[tempKey].Add(gameobject);
                            }
                        }
                    }
                }
            }
            else // roomIn != null
            {
                foreach (int tempKey in objects.Keys)
                {
                    if (!visibleObjects.ContainsKey(tempKey))
                        visibleObjects.Add(tempKey, new List<GameObject>());
                    foreach (GameObject gameobject in objects[tempKey])
                    {
                        if ((tempKey & GO_TYPE.SKYBOX) == GO_TYPE.SKYBOX)
                        {
                            visibleObjects[GO_TYPE.SKYBOX].Add(gameobject);
                            gameobject.position.T = Matrix.CreateTranslation(vantage.pos());
                        }
                        else
                        {
                            bool visible = false;
                            if (roomIn.area.Contains(gameobject.position.pos()) != ContainmentType.Disjoint)
                            {
                                visible = true;
                            }
                            else
                            {
                                foreach (Room room in roomIn.visibleRooms)
                                {
                                    if (room.area.Contains(gameobject.position.pos()) != ContainmentType.Disjoint)
                                    {
                                        visible = true;
                                        break;
                                    }
                                }
                            }
                            if (visible)
                            {
                                if ((tempKey & GO_TYPE.SCENERY) == GO_TYPE.SCENERY)
                                    gameobject.position.R = Quaternion.CreateFromRotationMatrix(Matrix.CreateBillboard(vantage.pos(), gameobject.position.pos(), Vector3.One, Vector3.Forward));
                                visibleObjects[tempKey].Add(gameobject);
                            }
                        }
                    }
                }
            }
            // Player specific visibility logic
            if (visibleObjects.ContainsKey(GO_TYPE.CHECKPOINT))
            {
                foreach (GameObject gameObject in visibleObjects[GO_TYPE.CHECKPOINT])
                {
                    RaceStatus status = Engine.GetInstance().race.status(player, true);
                    int lap = status.lap;
                    int currentLocation = status.checkpoint;
                    int checkpointPosition = ((Checkpoint)gameObject).racePosition;
                    if (checkpointPosition > currentLocation)
                    {
                        // TODO: change differentiation from visible/invisible to differences in how the checkpoints are rendered (color? brightness?)
                        gameObject.models[0].visible = true;
                        gameObject.models[1].visible = false;
                    }
                    else
                    {
                        gameObject.models[0].visible = false;
                        gameObject.models[1].visible = true;
                    }

                }
            }

            if (visibleObjects.ContainsKey(GO_TYPE.DIRECTIONAL_ARROW))
            {
                List<GameObject> removeTheseArrows = new List<GameObject>();
                foreach (GameObject gameObject in visibleObjects[GO_TYPE.DIRECTIONAL_ARROW])
                {
                    if(!player.arrow.Equals(gameObject)){
                        removeTheseArrows.Add(gameObject);
                    }
                }
                foreach (GameObject gameObject in removeTheseArrows)
                {
                    visibleObjects[GO_TYPE.DIRECTIONAL_ARROW].Remove(gameObject);
                }
            }

            List<GameObject> shownObjects = sortObjects(visibleObjects);
            if (player != null)
            {
                shownObjects.AddRange(player.dust);
            }
            return shownObjects;
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
                    foreach (GameObject gameobject in temp)
                    {
                        tObjects.Add(gameobject);
                    }
                }
            }
            return tObjects;
        }

        public Dictionary<int, List<GameObject>> sortOctTree(out Dictionary<int, List<GameObject>> entirelyVisible, BoundingFrustum viewFrustrum)
        {
            Dictionary<int, List<GameObject>> optimizedObjectDictionary = new Dictionary<int,List<GameObject>>();
            entirelyVisible = new Dictionary<int, List<GameObject>>();
            if (disableOctTree)
            {
                return objects;
            }
            List<GameObject> notVisible;
            List<GameObject> entirelyVisibleList = octTreeRoot.visible(out notVisible, viewFrustrum);
            foreach (int tempKey in objects.Keys)
            {
                entirelyVisible.Add(tempKey, new List<GameObject>());
                List<GameObject> objectTypeList = objects[tempKey];
                if (tempKey == GO_TYPE.SKYBOX || tempKey == GO_TYPE.DUST || tempKey == GO_TYPE.DIRECTIONAL_ARROW)
                {
                    entirelyVisible[tempKey].AddRange(objectTypeList);
                }
                else
                {
                    foreach (GameObject obj in notVisible)
                    {
                        if (objectTypeList.Contains(obj))
                        {
                            objectTypeList.Remove(obj);
                        }
                    }
                    foreach (GameObject obj in entirelyVisibleList)
                    {
                        if (objectTypeList.Contains(obj))
                        {
                            objectTypeList.Remove(obj);
                            entirelyVisible[tempKey].Add(obj);
                        }
                    }
                }
                optimizedObjectDictionary.Add(tempKey, objectTypeList);
            }
            return optimizedObjectDictionary;
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
                    outsideObj.health = 0;
                    output += "GameObject: " + outsideObj + " at position: " + outsideObj.position.pos() + " not contained within the OcTree.\n";
                }
            }
            if (debugBoundingBoxSize)
            {
                Console.WriteLine(output);
            }

        }

        public List<GameObject> sortObjects(Dictionary<int, List<GameObject>> likelyVisible)
        {

            //Add in order: SKYBOX/SCENERY objects first, PLAYER objects last

            List<GameObject> listedObjects = new List<GameObject>();
            foreach (int key in likelyVisible.Keys)
            {
                listedObjects.AddRange(likelyVisible[key]);
            }
            return listedObjects;
        }
    }
}
