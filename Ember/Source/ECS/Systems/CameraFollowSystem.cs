using Microsoft.Xna.Framework;
using Ember.Graphics;
using Ember.ECS.Components;

namespace Ember.ECS.Systems
{
    public class CameraFollowSystem : System
    {
        private readonly Camera _camera;

        public CameraFollowSystem(World world, Camera camera) : base(world, new Filter(world)
            .Include(typeof(Position),
                     typeof(CameraFollow)))
        {
            _camera = camera;
        }

        protected override void UpdateEntity(Entity entity, GameTime gameTime)
        {
            var position = entity.GetComponent<Position>();

            _camera.UpdateTransformMatrix(position.Value);
        }
    }
}
