using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A weapon is a gameobject that can fire projectiles and cause damage.
    /// Models for weapons should have the *bottom* of the weapon located at the origin where it will meet the ship/turret.
    /// </summary>
    class Weapon : GameObject
    {
        // Where projectiles are emitted, relative to the *weapon's* identity matrix
        public Coords fireFrom;
        public Bullet bullet;

        // Behaviors
        public Weapon() 
        {
            fireFrom = new Coords();
            bullet = new Bullet();
            bullet.damage = 10;
        }
    }
}
