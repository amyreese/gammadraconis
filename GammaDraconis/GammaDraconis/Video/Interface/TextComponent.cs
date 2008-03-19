using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GammaDraconis.Video.Interface
{
    class TextComponent : InterfaceComponent
    {
        private SpriteFont spriteFont;
        public String spriteFontName;
        public String text;
        public Color color;

        public TextComponent(GammaDraconis game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            if (spriteFontName != null)
            {
                spriteFont = Game.Content.Load<SpriteFont>(spriteFontName);
            }
            base.LoadContent();
        }

        internal override void Draw(GameTime gameTime, Vector2 position, Vector2 scale, float rotation)
        {
            if (spriteFont == null)
            {
                LoadContent();
            }
            if (spriteFont != null && text != null && color != null)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(spriteFont, text, RelativePosition, color, rotation + RelativeRotation, position, scale * RelativeScale, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }


    }
}
