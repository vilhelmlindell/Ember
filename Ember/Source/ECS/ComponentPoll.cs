using System.Collections.Generic;

namespace Ember.ECS
{
    public interface IComponentPool 
    {
        public void Add(long entityID, object component);
        public void Remove(long entityID);
    }

    public class ComponentPool<T> : IComponentPool
    {
        public Dictionary<long, T> ComponentsByEntityID;

        public ComponentPool()
        {
            ComponentsByEntityID = new Dictionary<long, T>();
        }

        public void Add(long entityID, object component)
        {
            ComponentsByEntityID.Add(entityID, (T)component);
        }
        public void Remove(long entityID)
        {
            ComponentsByEntityID.Remove(entityID);
        }
    }
}
