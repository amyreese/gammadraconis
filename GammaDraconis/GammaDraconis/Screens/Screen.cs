using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Video.GUI;
using GammaDraconis.Video;
using GammaDraconis.Core.Input;
using GammaDraconis.Types;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// The base that all screens displayed to the user inherit from
    /// </summary>
    abstract class Screen : DrawableGameComponent
    {
        /// <summary>
        /// The instance of the game that is using this screen
        /// </summary>
        protected GammaDraconis gammaDraconis;

        /// <summary>
        /// The interface for the screen
        /// </summary>
        protected Interface screenInterface;

        /// <summary>
        /// The scene for the screen
        /// </summary>
        protected Scene screenScene;

        /// <summary>
        /// Input manager
        /// </summary>
        protected static MenuInput input = new MenuInput();

        /// <summary>
        /// Creates a new Screen
        /// </summary>
        /// <param name="game">The instance of the game.</param>
        protected Screen(GammaDraconis game)
            : base(game)
        {
            gammaDraconis = game;
            screenScene = new Scene();
            screenInterface = new Interface(gammaDraconis);
            screenInterface.Visible = Visible;
        }

        /// <summary>
        /// Used as a flag to determine if we can transition to or from this screen or not
        /// </summary>
        private bool _ready = false;
        public bool ready
        {
            get
            {
                return _ready;
            }
            set
            {
                _ready = value;
            }
        }
        private bool _fresh = false;
        public bool Fresh
        {
            get
            {
                return _fresh;
            }
            set
            {
                _fresh = value;
            }
        }
        /// <summary>
        /// Updates the visible flag of the interface for this screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            screenInterface.Visible = Visible;
            base.OnVisibleChanged(sender, args);
        }

        /// <summary>
        /// Updates the enabled flag of the interface for this screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            screenInterface.Enabled = Enabled;
            base.OnEnabledChanged(sender, args);
        }

        /// <summary>
        /// Draw the screen
        /// </summary>
        /// <param name="gameTime">GameTime for this draw</param>
        /// <param name="clearScreen">Flag to determine if the screen should be cleared or not</param>
        protected void Draw(GameTime gameTime, bool clearScreenAndDrawScene)
        {
            if (clearScreenAndDrawScene)
            {
                GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.Black, 1.0f, 0);
                GammaDraconis.renderer.render(gameTime, screenScene, new Coords());
            }

            Vector2 scale = new Vector2(Game.Window.ClientBounds.Width / 1024.0f, Game.Window.ClientBounds.Height / 768.0f);
            screenInterface.Draw(gameTime, Vector2.Zero, scale, 0);
#if DEBUG
            gammaDraconis.DebugInterface.Draw(gameTime, Vector2.Zero, scale, 0.0f);
#endif
            base.Draw(gameTime);
        }

        protected virtual void onFreshLoad() { }

        /// <summary>
        /// Draw the screen
        /// </summary>
        /// <param name="gameTime">GameTime for this draw</param>
        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, true);
        }

        public override void Update(GameTime gameTime)
        {
            if (Fresh)
            {
                onFreshLoad();
                Fresh = false;
            }

            base.Update(gameTime);
        }
    }
}
