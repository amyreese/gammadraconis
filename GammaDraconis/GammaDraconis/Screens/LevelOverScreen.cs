using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Video.GUI;
using GammaDraconis.Core.Input;
using GammaDraconis.Core;
using GammaDraconis.Types;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// A screen displayed when the game is loading
    /// </summary>
    class LevelOverScreen : LoadingScreen
    {
        private Text loadingText;

        /// <summary>
        /// Constructor for the screen
        /// </summary>
        /// <param name="game">Snails Pace instance</param>
        public LevelOverScreen(GammaDraconis game)
            : base(game, GammaDraconis.GameStates.MainMenu)
        {
            loadingText = new Text(gammaDraconis);
            loadingText.color = Color.White;
            loadingText.text = "";
            loadingText.spriteFontName = "Resources/Fonts/Menu";
            loadingText.RelativePosition = new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            loadingText.center = true;
            screenInterface.AddComponent(loadingText);
        }

        /// <summary>
        /// Load the background image
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void LevelOver( Race race )
        {
            int place = 1;
            loadingText.text = "";
            foreach( Racer r in race.rankings() )
            {
                loadingText.text = loadingText.text + place++ + ". " + r.ToString() + "\n";
            }
            ready = false;
        }

        /// <summary>
        /// Modifies the color of the text gradually
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if( input.inputDown(MenuInput.Commands.Cancel ) )
            {
                ready = true;
            }
            base.Update(gameTime);
        }
    }
}
