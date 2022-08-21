using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.ECS.Components
{
    public class SpriteRenderer
    {
        public Texture2D Texture;
        public Rectangle? SourceRectangle;
        public Vector2 Origin = Vector2.Zero;
        public Vector2 Offset = Vector2.Zero;
        public Vector2 Scale = Vector2.One;
        public Color Color = Color.White;
        public SpriteEffects SpriteEffect = SpriteEffects.None;
        public float DestinationWidth;
        public float DestinationHeight;
        public float LayerDepth;
        public float Rotation;
        public bool IsTranslated = true;
    }
}
