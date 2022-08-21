using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Graphics
{
    public class Sprite
    {
        public Texture2D Texture;
        public Rectangle? SourceRectangle;

        public Sprite(Texture2D texture, Rectangle? sourceRectangle = null)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
        }
    }
}
