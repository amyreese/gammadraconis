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
    class GameObject
    {
        /// <summary>
        /// The instance of snails pace that is using this screen
        /// </summary>
        protected GammaDraconis game;

        public GameObject()
        {
            position = new Coords();
            velocity = new Coords();
            acceleration = new Coords();

            models = new List<FBXModel>();
            mounts = new List<MountPoint>();
            turrets = new List<Turret>();
        }

        // Movement properties
        public Coords position;
        public Coords velocity;
        public Coords acceleration;

        // Visual properties
        public List<FBXModel> models;

        // Locations to mount weapons
        public List<MountPoint> mounts;

        // Turrets for weapons
        public List<Turret> turrets;

        // Behaviors
        public virtual void think(GameTime gameTime) { }
    }
}
