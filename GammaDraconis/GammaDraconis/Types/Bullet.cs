using System;
using System.Collections.Generic;
using System.Text;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A bullet represents a projectile fired by a weapon.
    /// </summary>
    class Bullet : GameObject
    {
        public float damage;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="game">The instance of the game.</param>
        protected Bullet(GammaDraconis game) : base(game) { }
    }
}
