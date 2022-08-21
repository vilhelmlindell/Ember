using Microsoft.Xna.Framework;

namespace Ember.Animations
{
    public class AnimationFrame
    {
        public string FileName { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Vector2 Size { get; set; }
        public int Duration { get; set; }
    }
}
