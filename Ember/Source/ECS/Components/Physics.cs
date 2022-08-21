using Microsoft.Xna.Framework;

namespace Ember.ECS.Components
{
    public class Physics
    {
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 Force;
        public float Mass = 1f;
    }
}
