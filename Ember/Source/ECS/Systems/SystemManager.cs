using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Ember.ECS.Systems
{
    public class SystemManager
    {
        public readonly List<System> UpdateSystems = new List<System>();
        public readonly List<System> DrawSystems = new List<System>();

        public void Update(GameTime gameTime)
        {
            foreach (System system in UpdateSystems)
                system.Update(gameTime);
        }
        public void Draw(GameTime gameTime)
        {
            foreach (System system in DrawSystems)
                system.Update(gameTime);
        }
    }
}