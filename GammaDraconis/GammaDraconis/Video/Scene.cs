using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Types;

namespace GammaDraconis.Video
{
    /// <summary>
    /// Properties that can be set on any object added to the scene manager.
    /// eg: Scene.track(object, GO_TYPE.RACER | GO_TYPE.GHOST) 
    /// </summary>
    public static class GO_TYPE
    {
        static public int SCENERY = 1;  // Never checked for thinking or physics, always drawn first, uncollideable
        static public int GHOST = 2;  // Uncollidable, partially see-through?
        static public int RACER = 4;
        static public int BULLET = 8;

        // Composite
        static public int HUD = SCENERY | GHOST;
    }

    /// <summary>
    /// The scene manager holds the 'world' the game is contained within.
    /// Background scenery, game objects, and other such items should be kept here.
    /// </summary>
    class Scene
    {
        // References to all objects in the scene, *including* the player objects
        private List<GameObject> objects;

        /// <summary>
        /// Create a new Scene manager.
        /// </summary>
        public Scene()
        {
            objects = new List<GameObject>();
        }

        /// <summary>
        /// Add an existing item to the scene manager with a given set of options.
        /// </summary>
        /// <param name="gameObject">Item to be tracked</param>
        /// <param name="type">Item properties</param>
        public void track(GameObject gameObject, int type)
        {
            // TODO: use a more complex system to track all the object types
            objects.Add(gameObject);
        }

        /// <summary>
        /// Remove an existing item from the scene manager.
        /// </summary>
        /// <param name="gameObject">Item to be removed</param>
        public void ignore(GameObject gameObject)
        {
            if (objects.Contains(gameObject))
            {
                objects.Remove(gameObject);
            }
        }

        /// <summary>
        /// Return a list of GameObjects that are collidable.
        /// </summary>
        /// <returns>GameObjects to check for collision</returns>
        public List<GameObject> collidable()
        {
            // TODO: return an oct tree of objects
            return objects;
        }

        /// <summary>
        /// Return a list of GameObjects that should have think() called.
        /// </summary>
        /// <returns>GameObjects to think()</returns>
        public List<GameObject> thinkable()
        {
            // TODO: only return appropriate objects
            return objects;
        }

        /// <summary>
        /// Return a list of GameObjects that are within range and 
        /// viewing arc of the given vantage point coordinates.
        /// </summary>
        /// <param name="vantage">Vantage point Coords to render from</param>
        /// <returns>List of GameObjects to render</returns>
        public List<GameObject> visible(Coords vantage)
        {
            // TODO: only return objects that should actually be drawn for this vantage point.
            return objects;
        }
    }
}
