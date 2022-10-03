using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ember.Graphics;

namespace Ember.UI
{
    public class Image : Control
    {
        public Sprite Sprite;
        public Color Color = Color.White;
        public SpriteEffects SpriteEffect = SpriteEffects.None;
        public float Rotation;

        public Image(Sprite sprite)
        {
            Sprite = sprite;
            
            Width.Value = sprite.Texture.Width;
            Height.Value = sprite.Texture.Height;
        }

        public override void Draw(GraphicsContext graphicsContext, GameTime gameTime, Vector2 parentPosition)
        {
            graphicsContext.SpriteBatch.Draw(Sprite.Texture, new RectangleF(Position + parentPosition, ActualWidth, ActualHeight).Rectangle, 
                Sprite.SourceRectangle, Color, Rotation, Vector2.Zero, SpriteEffect, LayerDepth);

            base.Draw(graphicsContext, gameTime, parentPosition);
        }
    }
}
