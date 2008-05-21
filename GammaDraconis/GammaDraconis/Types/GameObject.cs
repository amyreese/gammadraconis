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

            relationalScale = 1.0f;
            relativeLookAt = new Vector3(0f, 0f, -1f);
            relativeLookFrom = new Vector3(0f, 0f, 0f);
        }

        public int fakeTransparency = -1;

        public GameObject ownedBy;

        // Movement properties
        public Coords position;
        public Coords velocity;
        public Coords acceleration;

        // Movement limits
        public float rateL = 1f;
        public float rateR = 1f;
        public float rateV = 100f;

        // Physical properties
        public int mass = 1000;
        public float dragL = 1f;
        public float dragR = 3f;
        public float size = 2f;

        // Camera settings
        public Coords camera;
        public Renderer.Viewports viewport;
        public Vector3 relativeLookAt;              //A Vector3 that determines the point relative to the ship the viewport is looking at
        public Vector3 relativeLookFrom;            //A Vector3 that determines the point relative to the ship the viewport is looking from
        public float relationalScale = 0.0f;               //A float that acts as an index for relativeLookAt and relativeLookFrom

        // Visual properties
        public List<FBXModel> models;
        public FBXModel shieldModel;
        public Explosion explosion;

        // Locations to mount weapons
        public List<MountPoint> mounts;

        // Turrets for weapons
        public List<Turret> turrets;

        // Health and other attributes
        public float health;
        public float maxHealth;
        public float shield;
        public float maxShield;
        public float shieldIncreaseRate;
        public bool invincible;
        public double shieldVisibilityTimer = 0;
        public bool immobile;

        //Sounds for movement and initial movement
        public String thrusterSFX = "";
        public String engine_startSFX = "";
        public String onDeathSound = "";

        public LuaInterface.LuaFunction OnDeathFunction;

        /// <summary>
        /// Clone a game object
        /// </summary>
        /// <returns>The cloned object</returns>
        public virtual GameObject clone()
        {
            GameObject go = new GameObject();

            go.mass = mass;
            go.size = size;

            if (explosion != null)
            {
                go.explosion = explosion.clone();
            }

            go.rateL = rateL;
            go.rateR = rateR;
            go.dragL = dragL;
            go.dragR = dragR;

            foreach (FBXModel model in models)
            {
                go.models.Add(model.clone());
            }

            if (shieldModel != null)
            {
                go.shieldModel = shieldModel.clone();
            }
            
            foreach(MountPoint mount in mounts)
            {
                go.mounts.Add(mount.clone());
            }

            foreach (Turret turret in turrets)
            {
                go.turrets.Add(turret.clone());
            }

            go.maxHealth = maxHealth;
            go.invincible = invincible;
            go.immobile = immobile;
            go.health = maxHealth;
            go.maxShield = maxShield;
            go.shield = maxShield;
            go.shieldIncreaseRate = shieldIncreaseRate;
            go.OnDeathFunction = OnDeathFunction;

            go.relativeLookAt = new Vector3(relativeLookAt.X, relativeLookAt.Y, relativeLookAt.Z);
            go.relativeLookFrom = new Vector3(relativeLookFrom.X, relativeLookFrom.Y, relativeLookFrom.Z);
            go.relationalScale = relationalScale;

            go.engine_startSFX = engine_startSFX;
            go.thrusterSFX = thrusterSFX;
            go.onDeathSound = onDeathSound;
            
            return go;
        }

        /// <summary>
        /// Scale a model by the specified factor.
        /// </summary>
        /// <param name="factor">The scaling factor.</param>
        public void scaleModels(float factor)
        {
            foreach (FBXModel model in models)
            {
                model.scale *= factor;
            }
            if (shieldModel != null)
            {
                shieldModel.scale *= factor;
            }
        }

        public virtual void test() { }

        // Behaviors
        public virtual void think(GameTime gameTime)
		{
			List<Weapon> weapons = getAllWeapons();
			foreach (Weapon weapon in weapons)
			{
				weapon.lastFired += Engine.gameTime.ElapsedGameTime.Milliseconds;
			}
		}

        /// <summary>
        /// Fire the ship's weapons.  By default, fires all primary weapons on the ship.
        /// </summary>
        public virtual void firePrimary() 
        {
            List<Weapon> weapons = getWeapons(W_TYPE.PRIMARY, true);
            fireWeapons(weapons);
        }

		/// <summary>
		/// Fire the ship's secondary weapons.
		/// </summary>
		public virtual void fireSecondary()
		{
			List<Weapon> weapons = getWeapons(W_TYPE.SECONDARY, true);
			fireWeapons(weapons);
		}

        /// <summary>
        /// Linearly accelerate the ship along the beam at the given amount of throttle.
        /// </summary>
        /// <param name="amount">Throttle amount clamped between -1f and 1f</param>
        public virtual void throttle(float amount)
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
        /// 
        /// </summary>
        /// <returns>Where the viewport is looking at</returns>
        public Matrix getCameraLookAtMatrix()
        {
            Coords c = getCamera();
            Matrix m = Matrix.CreateTranslation(relativeLookAt) * Matrix.CreateFromQuaternion(camera.R) * Matrix.CreateFromQuaternion(position.R) * position.T;
            return Matrix.CreateLookAt(c.pos() - velocity.pos() / 2, m.Translation, c.up());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Where the viewport is looking from</returns>
        public Coords getCamera()
        {
            Coords c = new Coords();
            c.R = position.R * camera.R;
            c.T = Matrix.CreateTranslation(relativeLookFrom) * Matrix.Invert(Matrix.CreateFromQuaternion(velocity.R)) * Matrix.CreateFromQuaternion(camera.R) * Matrix.CreateFromQuaternion(position.R) * position.T * Matrix.CreateTranslation(velocity.pos() * -0.4f);
            return c;
        }

        /// <summary>
        /// Make this object take 1 damage.
        /// </summary>
        public void takeDamage()
        {
            takeDamage(1);
        }

        /// <summary>
        /// Make this object take a specified amount of damage.
        /// </summary>
        /// <param name="damage">The amount of damage.</param>
        public void takeDamage(float damage)
        {
            if (!invincible && damage > 0)
            {
                if (shield > 0)
                {
                    shieldVisibilityTimer = Math.Max(shieldVisibilityTimer, 1);
                }
                shield -= damage;
                if (shield < 0)
                {
                    health += shield;
                    shield = 0;
                }
            }
        }

        /// <summary>
        /// Gather a list of weapons on the ship, optionally including
        /// weapons on turrets, and optionally translating weapon positions
        /// appropriately to their turret/mount positions.
        /// </summary>
		/// <param name="type">What type of weapons to get</param>
        /// <param name="translate">Translate the weapon positions</param>
        /// <param name="turrets">Return turreted weapons</param>
        /// <returns>List of weapons</returns>
		private List<Weapon> getWeapons(int type, bool translate, bool turretweapons)
        {
            Weapon w = null;
            List<Weapon> weapons = new List<Weapon>();

            // Gather and translate all ship-mounted weapons
            foreach (MountPoint mount in mounts)
            {
                w = mount.weapon;
                if (w != null && 0 != (w.type & type))
                {
                    if (translate)
                    {
                        w.position.T = mount.location.T * position.matrix();
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
						if (w != null && 0 != (w.type & type))
                        {
                            if (translate)
                            {
                                w.position.T = mount.location.T * turret.location.matrix() * position.matrix();
                                w.position.R = position.R * turret.location.R * mount.location.R;
                            }
                            weapons.Add(w);
                        }
                    }
                }
            }

            return weapons;
        }
		private List<Weapon> getWeapons() { return getWeapons(W_TYPE.PRIMARY, false, false); }
		private List<Weapon> getWeapons(int type) { return getWeapons(type, false, false); }
		private List<Weapon> getWeapons(int type, bool translate) { return getWeapons(type, translate, false); }
		private List<Weapon> getAllWeapons() { return getWeapons(W_TYPE.ALL, false, true); }
		private List<Weapon> getAllWeapons(bool translate) { return getWeapons(W_TYPE.ALL, translate, true); }

        /// <summary>
        /// Fires a list of weapons by cloning the weapon's bullet type and accelerating it, 
        /// then telling the scene manager to track it.
        /// </summary>
        /// <param name="weapons"></param>
        private void fireWeapons(List<Weapon> weapons)
        {
            foreach (Weapon weapon in weapons)
            {
                if (weapon.lastFired >= 0)
                {
					if ((weapon.type & W_TYPE.PRIMARY) != 0 || weapon.ammo > 0)
					{
						Bullet b = weapon.bullet.clone();
						b.ownedBy = this;
						b.position.T = weapon.position.matrix() * weapon.fireFrom.matrix();
						b.position.T = Matrix.CreateTranslation(b.position.pos());
						b.position.R = weapon.position.R * weapon.fireFrom.R;
						b.lastPosition = b.position.Clone();
						b.velocity = velocity.Clone();
						b.throttle(1f);
                        Audio.play(weapon.fireSFX);

						Engine.GetInstance().gameScene.track(b, GO_TYPE.BULLET);

						weapon.lastFired = -weapon.cooldown;

						if (0 != (weapon.type & W_TYPE.SECONDARY))
						{
							weapon.ammo--;
						}
					}
                }
            }
        }

        /// <summary>
        /// Get the amount of remaining secondary ammunition.
        /// </summary>
        /// <returns>The amount of remaining secondary ammunition.</returns>
        public virtual float getAmmo()
        {
            List<Weapon> weapons = getWeapons(W_TYPE.SECONDARY);
            float current = 0;
            float max = 0;
            foreach (Weapon w in weapons)
            {
                current += w.ammo;
                max += w.ammoMax;
            }
            if (current == 0 || max == 0)
            {
                return 0f;
            }
            return current / max;
        }

        /// <summary>
        /// Function called when an object "dies".
        /// </summary>
        public virtual void OnDeath()
        {
            Audio.play(onDeathSound);

            if (explosion != null)
            {
                explosion.explode(new Coords(position.T.Translation));
            }

            if (OnDeathFunction != null)
            {
                OnDeathFunction.Call(this);
            }
        }

        public float maxVelocity
        {
            get
            {
                return 3.4f;
            }
        }
    }
}
