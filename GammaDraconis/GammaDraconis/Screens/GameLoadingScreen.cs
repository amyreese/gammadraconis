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
        private Interface someInterface;

        /// <summary>
        /// Constructor for the screen
        /// </summary>
        /// <param name="game">Snails Pace instance</param>
        public GameLoadingScreen(GammaDraconis game)
            : base(game, GammaDraconis.GameStates.Game)
        {
        }

        /// <summary>
        /// Load the background image
        /// </summary>
        protected override void LoadContent()
        {
            if (someInterface == null)
            {
                someInterface = new Interface(gammaDraconis);
                SpriteComponent sprite = new SpriteComponent(gammaDraconis);
                sprite.textureName = "Resources/Textures/MenuBackgrounds/MainMenu";
                sprite.RelativePosition = new Vector2(70, 10);
                sprite.RelativeScale = new Vector2(0.5f, 1.5f);
                someInterface.AddComponent(sprite);

                TextComponent text = new TextComponent(gammaDraconis);
                text.color = Color.White;
                text.text = "Test text";
                text.spriteFontName = "Resources/Fonts/Menu";
                text.RelativeRotation = 0.2f;
                someInterface.AddComponent(text);

                Interface subInterface = new Interface(gammaDraconis);
                subInterface.RelativeRotation = 0.5f;
                subInterface.AddComponent(sprite);
                someInterface.AddComponent(subInterface);
                subInterface.AddComponent(someInterface);
            }
            base.LoadContent();
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            if (someInterface != null)
            {
                someInterface.Visible = Visible;
            }
            base.OnVisibleChanged(sender, args);
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (someInterface != null)
            {
                someInterface.Enabled = Enabled;
            }
            base.OnEnabledChanged(sender, args);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gammaDraconis.changeState(GammaDraconis.GameStates.MainMenu);
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// Draw the screen
        /// </summary>
        /// <param name="gameTime">GameTime for this draw</param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.Black, 1.0f, 0);
            someInterface.Draw(gameTime, Vector2.Zero, Vector2.One, 0);
        }
    }
}
