using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Video;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A game object represents any 3D object in the game at a specific location,
    /// with a velocity and acceleration, and a list of models used to display it.
    /// </summary>
    class GameObject : DrawableGameComponent
    {
        /// <summary>
        /// The instance of snails pace that is using this screen
        /// </summary>
        protected GammaDraconis gammaDraconis;

        /// <summary>
        /// Creates a new GameObject.
        /// </summary>
        /// <param name="game">The instance of the game.</param>
        protected GameObject(GammaDraconis game)
            : base(game)
        {
            gammaDraconis = game;
        }

        // Movement properties
        public Coords position;
        public Coords velocity;
        public Coords acceleration;

        // Visual properties
        public List<Model> models;

        // Behaviors
        public void think(GameTime gameTime) { }
    }
}
