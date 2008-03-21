using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// The base that all screens displayed to the user inherit from
    /// </summary>
    abstract class Screen : DrawableGameComponent
    {
        /// <summary>
        /// The instance of snails pace that is using this screen
        /// </summary>
        protected GammaDraconis gammaDraconis;

        /// <summary>
        /// The interface for the screen
        /// </summary>
        protected Interface screenInterface;

        /// <summary>
        /// Creates a new Screen
        /// </summary>
        /// <param name="game">The instance of Snails Pace</param>
        protected Screen(GammaDraconis game)
            : base(game)
        {
            gammaDraconis = game;
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
        protected void Draw(GameTime gameTime, bool clearScreen)
        {
            if (clearScreen)
            {
                GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.Black, 1.0f, 0);
            }
            Vector2 scale = new Vector2(Game.Window.ClientBounds.Width / 800.0f, Game.Window.ClientBounds.Height / 600.0f);
            screenInterface.Draw(gameTime, Vector2.Zero, scale, 0);
            base.Draw(gameTime);
        }


        /// <summary>
        /// Draw the screen
        /// </summary>
        /// <param name="gameTime">GameTime for this draw</param>
        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, true);
        }
    }
}
