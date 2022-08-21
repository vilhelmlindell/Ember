using System;
using Microsoft.Xna.Framework;
using Ember.ECS.Components;

namespace Ember.ECS.Systems
{
    public class PhysicsSystem : System
    {
        private bool _updateHorizontal = true;

        public PhysicsSystem(World world) : base(world, new Filter(world)
            .Include(typeof(Position),
                     typeof(Physics)))
        {
        }

        protected override void UpdateEntity(Entity entity, GameTime gameTime)
        {
            if (_updateHorizontal)
            {
                UpdateHorizontalPhysics(entity, gameTime);
            }
            else
            {
                UpdateVerticalPhysics(entity, gameTime);
            }
        }

        private void UpdateHorizontalPhysics(Entity entity, GameTime gameTime)
        {
            var position = entity.GetComponent<Position>();
            var physics = entity.GetComponent<Physics>();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * 60;

            position.Value.X += physics.Velocity.X * deltaTime + 0.5f * physics.Acceleration.X * deltaTime * deltaTime;
            physics.Acceleration.X += physics.Force.X / physics.Mass;
            physics.Velocity.X += physics.Acceleration.X * deltaTime;

            _updateHorizontal = !_updateHorizontal;
        }
        private void UpdateVerticalPhysics(Entity entity, GameTime gameTime)
        {
            var position = entity.GetComponent<Position>();
            var physics = entity.GetComponent<Physics>();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * 60;

            physics.Acceleration.Y += physics.Force.Y / physics.Mass;
            position.Value.Y += physics.Velocity.Y * deltaTime + 0.5f * physics.Acceleration.X * deltaTime * deltaTime;
            physics.Velocity.Y += physics.Acceleration.Y * deltaTime;

            _updateHorizontal = !_updateHorizontal;
        }
    }
}
