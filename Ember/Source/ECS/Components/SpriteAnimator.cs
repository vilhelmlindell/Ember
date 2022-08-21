using Ember.Animations;

namespace Ember.ECS.Components
{
    public class SpriteAnimator
    {
        public Animation Animation;
        public AnimationTag CurrentTag;
        public AnimationFrame CurrentFrame;
        public int AnimationTimer;
        public int FrameIndex;
        public bool TagChanged;
            
        public void Play(string tagName)
        {
            foreach (AnimationTag tag in Animation.Tags)
            {
                if (tag.Name == tagName)
                {
                    CurrentTag = tag;
                    TagChanged = true;
                }
            }
        }
    }
}
