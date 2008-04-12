using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Core;
using GammaDraconis.Core.Input;
using GammaDraconis.Video;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// Screen giving players the chance to join the race, 
    /// select their ship, and select their weapons.
    /// </summary>
    class PlayerJoinScreen : Screen
    {
        PlayerInput[] inputs;

        /// <summary>
        /// Initialize the player join screen.
        /// </summary>
        /// <param name="game">Reference to the game</param>
        public PlayerJoinScreen(GammaDraconis game)
            : base(game)
        {
            inputs = new PlayerInput[4];
            inputs[0] = new PlayerInput(PlayerIndex.One);
            inputs[1] = new PlayerInput(PlayerIndex.Two);
            inputs[2] = new PlayerInput(PlayerIndex.Three);
            inputs[3] = new PlayerInput(PlayerIndex.Four);
        }
    }
}
