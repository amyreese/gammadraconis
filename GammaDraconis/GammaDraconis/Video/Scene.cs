using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Types;

namespace GammaDraconis.Video
{
    public sealed enum GO_TYPE
    {
        SCENERY = 1,  // Always drawn first, uncollideable
        GHOST   = 2,  // Uncollidable
        RACER   = 4,  
        BULLET  = 8
    }
    /// <summary>
    /// The scene manager holds the 'world' the game is contained within.
    /// Background scenery, game objects, and other such items should be kept here.
    /// </summary>
    class Scene
    {
        // Reference to the player's GameObject
        public GameObject player;

        // References to all objects in the scene, *including* the player's object
        public List<GameObject> objects;

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
            objects.Add(gameObject);
        }

        /// <summary>
        /// Remove an existing item from the scene manager.
        /// </summary>
        /// <param name="gameObject">Item to be removed</param>
        public void ignore(GameObject gameObject)
        {
        }

        /// <summary>
        /// Using the scene's player object, return a list of GameObjects that are
        /// within range of the player that should be rendered.
        /// </summary>
        /// <returns>List of GameObjects to render</returns>
        public List<GameObject> visibleObjects()
        {
            return objects;
        }
    }
}
