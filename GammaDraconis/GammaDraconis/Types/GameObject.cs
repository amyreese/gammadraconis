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
        public float rateL = 100f;
        public float rateR = 100f;

        // Physical properties
        public int mass = 1000;
        public float dragL = 1f;
        public float dragR = 3f;

        // Visual properties
        public List<FBXModel> models;

        // Locations to mount weapons
        public List<MountPoint> mounts;

        // Turrets for weapons
        public List<Turret> turrets;

        // Behaviors
        public virtual void think(GameTime gameTime) { }

        /// <summary>
        /// Linearly accelerate the ship along the beam at the given amount of throttle.
        /// </summary>
        /// <param name="amount">Throttle amount clamped between -1f and 1f</param>
        public void throttle(float amount)
        {
            amount = -MathHelper.Clamp(amount, -1f, 1f);
            acceleration.T *= Matrix.CreateTranslation(0f, 0f, amount);
        }

        /// <summary>
        /// Rotationally accelerate to pitch the ship up or down.
        /// </summary>
        /// <param name="amount">Stick amount clamped between -1f and 1f</param>
        public void pitch(float amount)
        {
            amount = -MathHelper.Clamp(amount, -1f, 1f) * 0.1f;
            acceleration.R *= Quaternion.CreateFromAxisAngle(Vector3.Right, amount);
        }

        /// <summary>
        /// Rotationally accelerate to roll the ship clockwise or counter-clockwise.
        /// </summary>
        /// <param name="amount">Stick amount clamped between -1f and 1f</param>
        public void roll(float amount)
        {
            amount = -MathHelper.Clamp(amount, -1f, 1f) * 0.1f;
            acceleration.R *= Quaternion.CreateFromAxisAngle(Vector3.Backward, amount);
        }

        /// <summary>
        /// Rotationally accelerate to yaw the ship left or right.
        /// </summary>
        /// <param name="amount">Stick amount clamped between -1f and 1f</param>
        public void yaw(float amount)
        {
            amount = -MathHelper.Clamp(amount, -1f, 1f) * 0.1f;
            acceleration.R *= Quaternion.CreateFromAxisAngle(Vector3.Up, amount);
        }

        /// <summary>
        /// Rotationally accelerate in a combined roll/yaw movement.
        /// </summary>
        /// <param name="amount">Stick amount clamped between -1f and 1f</param>
        public void turn(float amount)
        {
            amount = -MathHelper.Clamp(amount, -1f, 1f) * 0.1f;
            acceleration.R *= Quaternion.CreateFromAxisAngle(Vector3.Up, 0.5f * amount); // yaw
            acceleration.R *= Quaternion.CreateFromAxisAngle(Vector3.Backward, amount);  // roll
        }

    }
}
