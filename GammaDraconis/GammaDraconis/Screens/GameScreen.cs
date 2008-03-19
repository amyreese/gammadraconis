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

namespace GammaDraconis.Screens
{
    /// <summary>
    /// The screen for the actual game
    /// </summary>
    class GameScreen : Screen
    {
        // The game engine being used
        private Engine engine;

        // Has the game has been started?
        private bool started = false;

        GammaDraconis game;

        /// <summary>
        /// Constructor for the screen
        /// </summary>
        /// <param name="game">Gamma Draconis Instance</param>
        public GameScreen(GammaDraconis game)
            : base(game)
        {
            this.game = game;
            ready = false;
            // TODO: Remove this when we have proper loading.
            ReloadEngine("");
        }

        #region Engine Loading
        /// <summary>
        /// Reload the engine, using the specified map
        /// </summary>
        /// <param name="map">The map to load</param>
        public void ReloadEngine(String map)
        {
            ready = false;
            started = false;
            this.map = map;
            new Thread(loadEngine).Start();
        }

        // The current map
        public String map;

        /// <summary>
        /// Start up the engine using the current map
        /// </summary>
        protected void loadEngine()
        {
            engine = new Engine(game, map);
            /*
            Core.Player.time = 0;
            switch (map)
            {
                case "Tree Fort":
                    Core.Player.timeLimit = 100;
                    break;
                case "Garden":
                    Core.Player.timeLimit = 360;
                    break;
                case "Garden2":
                    Core.Player.timeLimit = 300;
                    break;
                case "Credits":
                    Core.Player.timeLimit = 0;
                    break;
                default:
                    Core.Player.timeLimit = 500;
                    break;
            }
             * */
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
            base.Draw(gameTime);
        }

        /// <summary>
        /// If the game hasn't started yet, start it.
        /// Tell the engine to run the AI and physics
        /// </summary>
        /// <param name="gameTime">GameTime for this update</param>
        public override void Update(GameTime gameTime)
        {
            if (!started)
            {
                //Engine.player.load();
                engine = new Engine(game, "");
                started = true;
            }
            engine.Update(gameTime);

            // TODO: Remove this once the game engine actually has input
            if( Keyboard.GetState().IsKeyDown(Keys.Escape) )
            {
                gammaDraconis.changeState(GammaDraconis.GameStates.MainMenu);
            }
            base.Update(gameTime);
        }
    }
}
