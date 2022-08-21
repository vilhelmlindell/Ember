using Ember.ECS.Components;
using Microsoft.Xna.Framework;

namespace Ember.ECS.Systems
{
    public class SpriteAnimatorSystem : System
    {
        public SpriteAnimatorSystem(World world) : base(world, new Filter(world)
            .Include(typeof(SpriteRenderer),
                     typeof(SpriteAnimator)))
        {
        }

        protected override void UpdateEntity(Entity entity, GameTime gameTime)
        {
            var sprite = entity.GetComponent<SpriteRenderer>();
            var animator = entity.GetComponent<SpriteAnimator>();

            if (animator.TagChanged && animator.CurrentTag != null)
            {
               animator.FrameIndex = animator.CurrentTag.From; 
               ChangeFrame(sprite, animator);
            }
            animator.TagChanged = false;

            if (animator.CurrentTag != null)
            {
                animator.AnimationTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (animator.AnimationTimer > animator.CurrentFrame.Duration)
                {
                    if (animator.FrameIndex < animator.CurrentTag.To)
                    {
                        animator.FrameIndex++;
                        ChangeFrame(sprite, animator);
                    }
                    else
                    {
                        animator.FrameIndex = animator.CurrentTag.From;
                        ChangeFrame(sprite, animator);
                    }
                }
            }
        }
        private static void ChangeFrame(SpriteRenderer sprite, SpriteAnimator animator)
        {
            animator.AnimationTimer = 0;
            animator.CurrentFrame = animator.Animation.Frames[animator.FrameIndex];
            sprite.SourceRectangle = animator.CurrentFrame.SourceRectangle;
        }
    }
}
