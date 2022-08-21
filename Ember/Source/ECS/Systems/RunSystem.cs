using Ember.ECS.Components;
using Microsoft.Xna.Framework;

namespace Ember.ECS.Systems
{
    public class RunSystem : System
    {
        public RunSystem(World world, Filter filter) : base(world, new Filter(world)
            .Include(typeof(Position),
                     typeof(Physics),
                     typeof(Run)))
        {
        }

        protected override void UpdateEntity(Entity entity, GameTime gameTime)
        {
            var physics = entity.GetComponent<Physics>();
            var collider = entity.GetComponent<BoxCollider>();
            var movement = entity.GetComponent<PlayerMovement>();
        }
    }
}
