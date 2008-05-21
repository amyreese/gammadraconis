using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GammaDraconis.Video.GUI
{
    class Sprite : InterfaceComponent
    {
        private Texture2D texture;
        public String textureName;
        public Color color;

        // Source rectangle
        public bool useSource = false;
        public Rectangle source;

        public Sprite(GammaDraconis game)
            : base(game)
        {
            color = Color.White;
        }

        public Sprite(GammaDraconis game, Rectangle source)
            : base(game)
        {
            color = Color.White;
            this.source = source;
            useSource = true;
        }

        protected override void LoadContent()
        {
            if (textureName != null)
            {
                texture = Game.Content.Load<Texture2D>(textureName);
            }
            base.LoadContent();
        }

        internal override void Draw(GameTime gameTime, Vector2 position, Vector2 scale, float rotation)
        {
            if ((texture == null) && (textureName != null))
            {
                LoadContent();
            }
            CalculateResultingValues(position, scale, rotation, out position, out scale, out rotation);
            if (texture != null)
            {
                spriteBatch.Begin();
                if (useSource)
                {
                    spriteBatch.Draw(texture, position, source, color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(texture, position, null, color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                spriteBatch.End();
            }
        }


    }
}
