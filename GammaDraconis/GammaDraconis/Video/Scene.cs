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
        static public int SCENERY = 1; // Never checked for thinking or physics, always drawn first and facing the viewer, uncollideable
        static public int HUD = 2;
        static public int THINKABLE = 4;
        static public int MOVABLE = 8;
        static public int COLLIDABLE = 16;

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
            foreach (int tempKey in objects.Keys)
            {
                List<GameObject> atemp = (List<GameObject>)objects[tempKey];
                foreach (GameObject gameobject in atemp)
                {
                    //TODO: Only return visible obljects
                    temp.Add(gameobject);
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

        #region GameObject rendering
        public void render( GameTime gameTime, Coords position, Matrix cameraMatrix, float viewingAngle, float viewingDistance )
        {
            renderObjects(visible(position), cameraMatrix, viewingAngle, viewingDistance);
        }

        private void renderObjects(List<GameObject> objects, Matrix cameraMatrix, float viewingAngle, float viewingDistance )
        {
            Matrix worldMatrix = Matrix.Identity;
            Matrix objectMatrix, modelMatrix;

            foreach (GameObject gameObject in objects)
            {
                objectMatrix = worldMatrix * gameObject.position.matrix();

                foreach (FBXModel fbxmodel in gameObject.models)
                {
                    modelMatrix = Matrix.CreateScale(fbxmodel.scale) * objectMatrix * fbxmodel.offset.matrix();

                    Model model = fbxmodel.model;

                    // Copy any parent transforms.
                    Matrix[] transforms = new Matrix[model.Bones.Count];
                    model.CopyAbsoluteBoneTransformsTo(transforms);
                    // Draw the model. A model can have multiple meshes, so loop.
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        // This is where the mesh orientation is set, as well as our camera and projection.
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.World = transforms[mesh.ParentBone.Index] * modelMatrix;
                            //effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                            effect.View = cameraMatrix;
                            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(viewingAngle),
                                GammaDraconis.GetInstance().GraphicsDevice.Viewport.AspectRatio, 1.0f, viewingDistance);
                        }
                        // Draw the mesh, using the effects set above.
                        mesh.Draw();
                    }
                }
            }
        }
        #endregion

    }
}
