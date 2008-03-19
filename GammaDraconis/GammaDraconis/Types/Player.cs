using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A Player is a special racer that is controlled by a human.
    /// </summary>
    class Player : Racer
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="game">The instance of the game.</param>
        protected Player(GammaDraconis game) : base(game) { }
    }
}
