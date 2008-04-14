using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GammaDraconis.Video
{
    class MotionBlurComponent : DrawableGameComponent
    {
        Effect motionBlurEffect;
        SpriteBatch spriteBatch;
        ResolveTexture2D resolveTarget;
        RenderTarget2D renderTarget;

        public MotionBlurComponent(Game game)
            : base(game)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            game.Components.Add(this);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            motionBlurEffect = Game.Content.Load<Effect>("Resources/Effects/RadialBlur");

            // Look up the resolution and format of our main backbuffer.
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;

            SurfaceFormat format = pp.BackBufferFormat;

            // Create a texture for reading back the backbuffer contents.
            resolveTarget = new ResolveTexture2D(GraphicsDevice, width, height, 1,
                format);

            renderTarget = new RenderTarget2D(GraphicsDevice, width, height, 1,
                format);
        }

        protected override void UnloadContent()
        {
            resolveTarget.Dispose();
            renderTarget.Dispose();
        }

        public void Render(float blurIntensity)
        {
            // Resolve the scene into a texture, so we can
            // use it as input data for the bloom processing.
            GraphicsDevice.ResolveBackBuffer(resolveTarget);

            // Pass 1: draw the scene into rendertarget 1, using a
            // shader that extracts only the brightest parts of the image.
            motionBlurEffect.Parameters["blurIntensity"].SetValue(blurIntensity);

            spriteBatch.Begin(SpriteBlendMode.None,
                  SpriteSortMode.Immediate,
                  SaveStateMode.None);

            // Begin the custom effect, if it is currently enabled. If the user
            // has selected one of the show intermediate buffer options, we still
            // draw the quad to make sure the image will end up on the screen,
            // but might need to skip applying the custom pixel shader.

            motionBlurEffect.Begin();
            motionBlurEffect.CurrentTechnique.Passes[0].Begin();
            

            // Draw the quad.
            spriteBatch.Draw(resolveTarget, new Rectangle(0, 0, renderTarget.Width, renderTarget.Height), Color.White);
            spriteBatch.End();

            // End the custom effect.

                motionBlurEffect.CurrentTechnique.Passes[0].End();
                motionBlurEffect.End();

        }
    }
}
