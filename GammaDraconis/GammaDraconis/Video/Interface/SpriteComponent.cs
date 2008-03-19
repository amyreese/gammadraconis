using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GammaDraconis.Video.Interface
{
    class SpriteComponent : InterfaceComponent
    {
        private Texture2D texture;
        public String textureName;

        public SpriteComponent(GammaDraconis game)
            : base(game)
        {
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
            if (texture == null)
            {
                LoadContent();
            }
            if (texture != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, RelativePosition, null, Color.White, rotation + RelativeRotation, position, scale * RelativeScale, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }


    }
}
