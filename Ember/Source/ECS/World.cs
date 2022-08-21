using Microsoft.Xna.Framework;
using Ember.ECS.Systems;

namespace Ember.ECS
{
    public class World
    {
        public readonly int MaxComponentTypes;
        public readonly EntityManager EntityManager;
        public readonly ComponentManager ComponentManager;
        public readonly SystemManager SystemManager;

        public World(int maxComponentTypes)
        {
            MaxComponentTypes = maxComponentTypes;
            EntityManager = new EntityManager(this);
            ComponentManager = new ComponentManager(this);
            SystemManager = new SystemManager();
        }

        public void Update(GameTime gameTime)
        {
            SystemManager.Update(gameTime);
        }
        public void Draw(GameTime gameTime)
        {
            SystemManager.Draw(gameTime);
        }
    }
}
