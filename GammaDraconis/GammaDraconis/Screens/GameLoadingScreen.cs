using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Video.Interface;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// A screen displayed when the game is loading
    /// </summary>
    class GameLoadingScreen : LoadingScreen
    {
        /// <summary>
        /// Constructor for the screen
        /// </summary>
        /// <param name="game">Snails Pace instance</param>
        public GameLoadingScreen(GammaDraconis game)
            : base(game, GammaDraconis.GameStates.Game)
        {
            SpriteComponent sprite = new SpriteComponent(gammaDraconis);
            sprite.textureName = "Resources/Textures/MenuBackgrounds/MainMenu";
            sprite.RelativePosition = new Vector2(70, 10);
            sprite.RelativeScale = new Vector2(0.5f, 1.5f);
            screenInterface.AddComponent(sprite);

            TextComponent text = new TextComponent(gammaDraconis);
            text.color = Color.White;
            text.text = "Test text";
            text.spriteFontName = "Resources/Fonts/Menu";
            text.RelativeRotation = 0.2f;
            screenInterface.AddComponent(text);

            Interface subInterface = new Interface(gammaDraconis);
            subInterface.RelativeRotation = 0.5f;
            subInterface.AddComponent(sprite);
            screenInterface.AddComponent(subInterface);
            interfaceReady = true;
        }

        /// <summary>
        /// Load the background image
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gammaDraconis.changeState(GammaDraconis.GameStates.MainMenu);
            }
            base.Update(gameTime);
        }
    }
}
