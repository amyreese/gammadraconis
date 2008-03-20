using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Core.Input;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A Player is a special racer that is controlled by a human.
    /// </summary>
    class Player : Racer
    {
        public static Player[] players = new Player[4];

        public PlayerIndex index;
        public PlayerInput input;

        public Player(PlayerIndex index)
            : base()
        {
            this.index = index;
            input = new PlayerInput(index);

            Player.players[(int)index] = this;
        }

        public override void think(GameTime gameTime)
        {
            input.update();

            float rate = 1f / gameTime.ElapsedGameTime.Milliseconds;

            if (input.inputDown("Up"))
            {
                position.A += rate;
            }
            if (input.inputDown("Down"))
            {
                position.A -= rate;
            }
            if (input.inputDown("Left"))
            {
                position.B -= rate;
            }
            if (input.inputDown("Right"))
            {
                position.B += rate;
            }
        }
    }
}
