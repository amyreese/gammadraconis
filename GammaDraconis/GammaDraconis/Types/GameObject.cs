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
        public float rateL = 200f;
        public float rateR = 100f;

        // Physical properties
        public int mass = 1000;
        public float dragL = 1f;
        public float dragR = 3f;
        public float size = 1f;

        // Visual properties
        public List<FBXModel> models;

        // Locations to mount weapons
        public List<MountPoint> mounts;

        // Turrets for weapons
        public List<Turret> turrets;

        /// <summary>
        /// Clone a game object
        /// </summary>
        /// <returns>The cloned object</returns>
        public virtual GameObject clone()
        {
            GameObject go = new GameObject();

            go.mass = mass;
            go.size = size;

            go.rateL = rateL;
            go.rateR = rateR;
            go.dragL = dragL;
            go.dragR = dragR;

            foreach (FBXModel model in models)
            {
                go.models.Add(model.clone());
            }
            
            foreach(MountPoint mount in mounts)
            {
                go.mounts.Add(mount.clone());
            }

            foreach (Turret turret in turrets)
            {
                go.turrets.Add(turret.clone());
            }

            return go;
        }

        // Behaviors
        public virtual void think(GameTime gameTime) { }

        /// <summary>
        /// Fire the ship's weapons.  By default, fires all weapons on the ship.
        /// </summary>
        public virtual void fire() 
        {
            List<Weapon> weapons = getWeapons(true);
            fireWeapons(weapons);
        }

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
            acceleration.R *= Quaternion.CreateFromAxisAngle(Vector3.Up, 1.0f * amount); // yaw
            acceleration.R *= Quaternion.CreateFromAxisAngle(Vector3.Backward, 0.4f * amount);  // roll
        }

        /// <summary>
        /// Gather a list of weapons on the ship, optionally including
        /// weapons on turrets, and optionally translating weapon positions
        /// appropriately to their turret/mount positions.
        /// </summary>
        /// <param name="translate">Translate the weapon positions</param>
        /// <param name="turrets">Return turreted weapons</param>
        /// <returns>List of weapons</returns>
        private List<Weapon> getWeapons(bool translate, bool turretweapons)
        {
            Weapon w = null;
            List<Weapon> weapons = new List<Weapon>();

            // Gather and translate all ship-mounted weapons
            foreach (MountPoint mount in mounts)
            {
                w = mount.weapon;
                if (w != null)
                {
                    if (translate)
                    {
                        w.position.T = position.matrix() * mount.location.T;
                        w.position.R = position.R * mount.location.R;
                    }
                    weapons.Add(w);
                }
            }

            // Gather and translate all turret-mounted weapons
            if (turretweapons)
            {
                foreach (Turret turret in turrets)
                {
                    foreach (MountPoint mount in mounts)
                    {
                        w = mount.weapon;
                        if (w != null)
                        {
                            if (translate)
                            {
                                w.position.T = position.matrix() * turret.location.matrix() * mount.location.T;
                                w.position.R = position.R * turret.location.R * mount.location.R;
                            }
                            weapons.Add(w);
                        }
                    }
                }
            }

            return weapons;
        }
        private List<Weapon> getWeapons() { return getWeapons(false, false); }
        private List<Weapon> getWeapons(bool translate) { return getWeapons(translate, false); }
        private List<Weapon> getAllWeapons() { return getWeapons(false, true); }
        private List<Weapon> getAllWeapons(bool translate) { return getWeapons(translate, true); }

        /// <summary>
        /// Fires a list of weapons by cloning the weapon's bullet type and accelerating it, 
        /// then telling the scene manager to track it.
        /// </summary>
        /// <param name="weapons"></param>
        private void fireWeapons(List<Weapon> weapons)
        {
            foreach (Weapon weapon in weapons)
            {
                Bullet b = weapon.bullet.clone();
                b.position.T = weapon.position.matrix() * weapon.fireFrom.matrix();
                b.position.T = Matrix.CreateTranslation(b.position.pos());
                b.position.R = weapon.position.R * weapon.fireFrom.R;
                b.throttle(1f);

                Engine.GetInstance().gameScene.track(b, GO_TYPE.BULLET);
            }
        }
    }
}
