using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Core;
using GammaDraconis.Video;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A game object represents any 3D object in the game at a specific location,
    /// with a velocity and acceleration, and a list of models used to display it.
    /// </summary>
    class GameObject
    {
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

        // Movement limits
        public float rateA = 25f;
        public float rateR = 0.1f;

        // Physical properties
        public int mass = 1000;
        public float drag = 1.0f;

        // Visual properties
        public List<FBXModel> models;

        // Locations to mount weapons
        public List<MountPoint> mounts;

        // Turrets for weapons
        public List<Turret> turrets;

        // Behaviors
        public virtual void think(GameTime gameTime) { }

        public void throttle(float amount)
        {
            amount = MathHelper.Clamp(amount, -1f, 1f) / Engine.gameTime.ElapsedGameTime.Milliseconds;
            acceleration.T *= Matrix.CreateTranslation(0f, 0f, -rateA * amount);
        }

        public void pitch(float amount)
        {
            amount = MathHelper.Clamp(amount, -1f, 1f) / Engine.gameTime.ElapsedGameTime.Milliseconds;
            acceleration.R *= Quaternion.CreateFromAxisAngle(Vector3.Right, -rateR * amount);
        }

        public void roll(float amount)
        {
            amount = MathHelper.Clamp(amount, -1f, 1f) / Engine.gameTime.ElapsedGameTime.Milliseconds;
            acceleration.R *= Quaternion.CreateFromAxisAngle(Vector3.Backward, -rateR * amount);
        }

        public void yaw(float amount)
        {
            amount = MathHelper.Clamp(amount, -1f, 1f) / Engine.gameTime.ElapsedGameTime.Milliseconds;
            acceleration.R *= Quaternion.CreateFromAxisAngle(Vector3.Up, -rateR * amount);
        }
    }
}
