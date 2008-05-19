using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GammaDraconis.Types
{
	class W_TYPE
	{
		static public int PRIMARY = 1;
		static public int SECONDARY = 2;

		static public int ALL = PRIMARY | SECONDARY;
	}

	/// <summary>
    /// A weapon is a gameobject that can fire projectiles and cause damage.
    /// Models for weapons should have the *bottom* of the weapon located at the origin where it will meet the ship/turret.
    /// </summary>
    class Weapon : GameObject
    {
		// Weapon type
		public int type = W_TYPE.PRIMARY;

        // Where projectiles are emitted, relative to the *weapon's* identity matrix
        public Coords fireFrom;
        public Bullet bullet;

        // Cooldown in milleseconds
        public int cooldown = 100;
        public int lastFired = 0;

		// Ammunition
		public int ammoMax = 1;
		public int ammo = 1;

        //Sound FX
        public String fireSFX = "";
        public String impactSFX = "";

        // Behaviors
        public Weapon() 
        {
            fireFrom = new Coords();
            bullet = new Bullet();
            bullet.damage = 10;
            bullet.timeToLive = 5;
            bullet.mass = 5;
        }

        public Weapon clone()
        {
            Weapon weapon = new Weapon();
			weapon.type = type;
			weapon.ammoMax = ammoMax;
			weapon.ammo = ammoMax;
			weapon.cooldown = cooldown;
            weapon.fireFrom = fireFrom;
            weapon.bullet = bullet.clone();
            weapon.fireSFX = fireSFX;
            weapon.impactSFX = impactSFX;

            weapon.onDeathSound = onDeathSound;

            return weapon;
        }
    }
}
