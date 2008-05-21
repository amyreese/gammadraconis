using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// A screen displayed when the game is loading
    /// </summary>
    class LevelLoadingScreen : LoadingScreen
    {
        private Text loadingText;
        private String loadingTextValue = "Loading";
        private int loadingTextDots = 0;
        private int maxDotCount = 3;
        private double timeSinceLastDot = 0;
        private double timeBetweenDots = 0.25;

        /// <summary>
        /// Constructor for the screen
        /// </summary>
        /// <param name="game">The instance of the game.</param>
        public LevelLoadingScreen(GammaDraconis game)
            : base(game, GammaDraconis.GameStates.Game)
        {
            loadingText = new Text(gammaDraconis);
            loadingText.color = Color.White;
            loadingText.text = loadingTextValue;
            loadingText.SpriteFontName = "Resources/Fonts/Menu";
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

        /// <summary>
        /// Modifies the color of the text gradually
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            timeSinceLastDot += gameTime.ElapsedRealTime.TotalSeconds;
            if (timeSinceLastDot > timeBetweenDots)
            {
                loadingText.text = loadingTextValue.PadRight(loadingTextValue.Length + loadingTextDots, '.');
                loadingTextDots++;
                loadingTextDots %= maxDotCount + 1;
                timeSinceLastDot -= timeBetweenDots;
            }
            base.Update(gameTime);
        }
    }
}
