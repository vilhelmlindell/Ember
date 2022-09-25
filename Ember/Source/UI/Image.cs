using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ember.Graphics;

namespace Ember.UI
{
    public class Image : Element
    {
        public Sprite Sprite;
        public Color Color = Color.White;
        public SpriteEffects SpriteEffect = SpriteEffects.None;
        public float Rotation;

        public Image(UIManager uiManager, Sprite sprite = null) : base(uiManager)
        {
            Sprite = sprite;
            
            if (sprite != null)
            {
                Width = sprite.Texture.Width;
                Height = sprite.Texture.Height;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Sprite != null)
            {
                spriteBatch.Draw(Sprite.Texture, AbsoluteBounds.Rectangle, Sprite.SourceRectangle,
                    Color, Rotation, Origin, SpriteEffect, LayerDepth);
            }

            base.Draw(spriteBatch, gameTime);
        }
    }
}
