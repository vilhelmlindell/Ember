using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ember.Graphics;

namespace Ember.GUI
{
    public class Image : Element
    {
        public Sprite Sprite;
        public Color Color = Color.White;
        public SpriteEffects SpriteEffect = SpriteEffects.None;
        public float Rotation;

        public Image() { }
        public Image(Sprite sprite)
        {
            Sprite = sprite;
            Width = sprite.Texture.Width;
            Height = sprite.Texture.Height;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Sprite == null) return;

            spriteBatch.Draw(Sprite.Texture, AbsoluteBounds.Rectangle, Sprite.SourceRectangle, 
                             Color, Rotation, Origin, SpriteEffect, LayerDepth);

            base.Draw(spriteBatch, gameTime);
        }
    }
}
