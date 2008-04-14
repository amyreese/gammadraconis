using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using GammaDraconis.Core;
using GammaDraconis.Types;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// The screen for the actual game
    /// </summary>
    class GameScreen : Screen
    {
        // The game engine being used
        private Engine engine;

        /// <summary>
        /// Constructor for the screen
        /// </summary>
        /// <param name="game">Gamma Draconis Instance</param>
        public GameScreen(GammaDraconis game)
            : base(game)
        {
            ready = false;
        }

        #region Engine Loading
        /// <summary>
        /// Reload the engine, using the specified map
        /// </summary>
        /// <param name="map">The map to load</param>
        public void ReloadEngine(String map, List<Player> players )
        {
            ready = false;
            this.map = map;
            this.players = players;
            loadEngine();
            // TODO: do this in a thread once we work out Lua threading issues
            //new Thread(loadEngine).Start();
        }

        // The current map
        public String map;
        private List<Player> players;

        /// <summary>
        /// Start up the engine using the current map
        /// </summary>
        protected void loadEngine()
        {
            engine = new Engine(gammaDraconis, map, players);
            ready = true;
        }
        #endregion

        /// <summary>
        /// Tell the engine that rendering should be done
        /// </summary>
        /// <param name="gameTime">GameTime for this draw</param>
        public override void Draw(GameTime gameTime)
        {
            engine.Render(gameTime);
            base.Draw(gameTime, false);
        }

        /// <summary>
        /// If the game hasn't started yet, start it.
        /// Tell the engine to run the AI and physics
        /// </summary>
        /// <param name="gameTime">GameTime for this update</param>
        public override void Update(GameTime gameTime)
        {
            engine.Update(gameTime);

            // TODO: Remove this once the game engine actually has input
            if( input.inputPressed(Core.Input.MenuInput.Commands.Cancel) )
            {
                gammaDraconis.changeState(GammaDraconis.GameStates.MainMenu);
            }
            base.Update(gameTime);
        }
    }
}
