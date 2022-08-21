using Ember.ECS.Components;
using Microsoft.Xna.Framework;

namespace Ember.ECS.Systems
{
    public class JumpSystem : System
    {
        public JumpSystem(World world) : base(world, new Filter(world)
            .Include(typeof(Position),
                     typeof(Physics),
                     typeof(BoxCollider),
                     typeof(Jump)))
        {
        }

        protected override void UpdateEntity(Entity entity, GameTime gameTime)
        {
            var position = entity.GetComponent<Position>();
            var physics = entity.GetComponent<Physics>();
            var collider = entity.GetComponent<BoxCollider>();
            var movement = entity.GetComponent<Jump>();
            

        }
    }
}
