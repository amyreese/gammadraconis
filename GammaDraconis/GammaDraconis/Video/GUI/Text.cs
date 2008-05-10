using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GammaDraconis.Video.GUI
{
    class Text : InterfaceComponent
    {
        private SpriteFont spriteFont;
        private String spriteFontName;
        private bool spriteFontNameChanged;
        public String text;
        public Color color;
        public bool center = false;

        public Text(GammaDraconis game)
            : base(game)
        {
        }

        public Text(GammaDraconis game, string text)
            : base(game)
        {
            this.text = text;
        }

        public string SpriteFontName
        {
            get { return spriteFontName; }

            set
            {
                if (spriteFontName != value)
                {
                    spriteFontName = value;
                    spriteFontNameChanged = true;
                }
            }
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
            if ((spriteFont == null) || spriteFontNameChanged)
            {
                LoadContent();
                spriteFontNameChanged = false;
            }
            CalculateResultingValues(position, scale, rotation, out position, out scale, out rotation);
            if (spriteFont != null && text != null && color != null)
            {
                Vector2 origin = Vector2.Zero;
                if (center)
                {
                    origin = spriteFont.MeasureString(text) / 2;
                }
                spriteBatch.Begin();
                spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }


    }
}
