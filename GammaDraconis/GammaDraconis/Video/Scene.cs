using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Types;

namespace GammaDraconis.Video
{
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

        public Scene()
        {
            objects = new List<GameObject>();
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
