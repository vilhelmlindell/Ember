using Microsoft.Xna.Framework;

namespace Ember.ECS.Systems
{
    public interface ISystem
    {
        public bool IsEnabled { get; set; }

        public void Update(GameTime gameTime);
    }
}
