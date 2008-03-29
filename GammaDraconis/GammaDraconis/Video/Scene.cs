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

        static public int RACER = NORMAL;
        static public int BULLET = NORMAL ^ THINKABLE;
        static public int COURSE = GHOST;
    }

    /// <summary>
    /// The scene manager holds the 'world' the game is contained within.
    /// Background scenery, game objects, and other such items should be kept here.
    /// </summary>
    class Scene
    {
        
        // References to all objects in the scene, *including* the player objects
        private Hashtable objects;
        
        /// <summary>
        /// Create a new Scene manager.
        /// </summary>
        public Scene()
        {
            objects = new Hashtable();
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

            List<GameObject> temp = new List<GameObject>();
            List<GameObject> tempScenery = new List<GameObject>();
            float aspRatio = GammaDraconis.renderer.aspectRatio;
            float viewAngle = GammaDraconis.renderer.viewingAngle;
            float viewDist = GammaDraconis.renderer.viewingDistance;

            // TODO: Figure out what the proper matrices for this are.
            BoundingFrustum viewFrustum = new BoundingFrustum(vantage.camera() * Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(viewAngle), aspRatio, 0.1f, viewDist));
            
            foreach (int tempKey in objects.Keys)
            {
                List<GameObject> atemp = (List<GameObject>)objects[tempKey];
                foreach (GameObject gameobject in atemp)
                {
                    // Take care of some quick cases before doing any math.
                    if(typedObjects(GO_TYPE.SKYBOX).Contains(gameobject))
                    {
                        temp.Add(gameobject);
                    }
                    else
                    {
                        // TODO: Use size of the object
                        if (viewFrustum.Contains(new BoundingSphere(gameobject.position.pos(), 1.0f)) != ContainmentType.Disjoint)
                        {
                            temp.Add(gameobject);
                        }
                        /*
                        //TODO: Verify I'm using the correct matrices.
                        Vector3 goVector = gameobject.position.matrix().Translation;
                        Vector3 vantVector = vantage.matrix().Forward;
                        float dx = goVector.X * vantVector.X > 0 ? goVector.X - vantVector.X : -1 * (goVector.X - vantVector.X);
                        float dy = goVector.Y * vantVector.Y > 0 ? goVector.Y - vantVector.Y : -1 * (goVector.Y - vantVector.Y);
                        float dz = goVector.Z * vantVector.Z > 0 ? goVector.Z - vantVector.Z : -1 * (goVector.Z - vantVector.Z);
                        
                        float frustX = (float)Math.Tan(viewAngle/2)*dz;
                        float frustY = frustX * aspRatio;

                        if ((0 < dz || dz < viewDist) && dx < frustX && dy < frustY)
                        {
                            temp.Add(gameobject);
                        }
                         * */

                        /*
                        //TODO: Look at objects size
                        else if ((0< dz + gammeobject.size || dz - gammeobject.size ) && dx - gammeobject.size < frustX && dy - gammeobject.size < frustY)
                        {
                            temp.Add(gameobject);
                        }*/
                        //Uncomment this to see unsorted results
                        /*else
                        {
                            temp.Add(gameobject);
                        }*/
                    }
                }
            }
            return temp;
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

    }
}
