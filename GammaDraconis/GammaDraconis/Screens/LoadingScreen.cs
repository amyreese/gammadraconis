using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// A screen that waits for another screen to be ready before transitioning to that screen
    /// </summary>
    abstract class LoadingScreen : Screen
    {
        /// <summary>
        /// The game state that the loading screen transitions into
        /// </summary>
        protected GammaDraconis.GameStates nextState;

        /// <summary>
        /// Sets up the loading screen
        /// </summary>
        /// <param name="game">The Gamma Draconis instance</param>
        /// <param name="nextState">The game state that the loading screen transitions into</param>
        protected LoadingScreen(GammaDraconis game, GammaDraconis.GameStates nextState)
            : base(game)
        {
            this.nextState = nextState;
            ready = true;
        }

        /// <summary>
        /// Checks to see if the next screen is ready, and if the loading screen is ready.
        /// If they're both ready, tells the game to move to the next screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (gammaDraconis.getScreen(nextState).ready && ready)
            {
                gammaDraconis.changeState(nextState);
            }
        }
    }
}
