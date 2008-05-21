using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Video;
using GammaDraconis.Video.GUI;
using GammaDraconis.Core.Input;
using GammaDraconis.Core;
using GammaDraconis.Types;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// A screen that displays race results.
    /// </summary>
    class LevelOverScreen : LoadingScreen
    {
        private Text resultText;
        private Text titleText;
        private GameObject skybox;

        /// <summary>
        /// Constructor for the screen.
        /// </summary>
        /// <param name="game">The instance of the game.</param>
        public LevelOverScreen(GammaDraconis game)
            : base(game, GammaDraconis.GameStates.MainMenu)
        {
            resultText = new Text(gammaDraconis);
            resultText.color = Color.White;
            resultText.text = "";
            resultText.SpriteFontName = "Resources/Fonts/Menu";
            resultText.RelativePosition = new Vector2(1024 / 2, 768 / 2);
            resultText.center = true;
            screenInterface.AddComponent(resultText);

            titleText = new Text(gammaDraconis, "Race Results");
            titleText.color = Color.White;
            titleText.SpriteFontName = "Resources/Fonts/Title";
            titleText.center = true;
            titleText.RelativePosition = new Vector2(1024 / 2, 50);
            titleText.RelativeScale = new Vector2(0.9f, 0.9f);
            screenInterface.AddComponent(titleText);

            skybox = new Skybox();
            screenScene.track(skybox, GO_TYPE.SKYBOX);
        }

        /// <summary>
        /// Load any images.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// Method called at the end of a race to display high score data.
        /// </summary>
        /// <param name="race">The race manager object.</param>
        public void LevelOver( Race race )
        {
            int place = 1;
            resultText.text = "";
            foreach( Racer r in race.rankings() )
            {
                resultText.text = resultText.text + place++ + ". " + r.ToString() + " - " + TimeSpan.FromMilliseconds(r.time).ToString().Substring(0, 11) + "\n";
            }
            ready = false;
        }

        /// <summary>
        /// Watches for the user to exit the menu.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if(input.inputPressed(MenuInput.Commands.Cancel) || input.inputPressed(MenuInput.Commands.Select))
            {
                ready = true;
            }
            base.Update(gameTime);
        }
    }
}
