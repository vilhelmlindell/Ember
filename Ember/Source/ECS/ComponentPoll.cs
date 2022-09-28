using System.Collections.Generic;

namespace Ember.ECS
{
    public interface IComponentPool 
    {
        public void Add(long entityId, object component);
        public void Remove(long entityId);
    }

    public class ComponentPool<T> : IComponentPool
    {
        public Dictionary<long, T> ComponentsByEntityId;

        public ComponentPool()
        {
            ComponentsByEntityId = new Dictionary<long, T>();
        }

        public void Add(long entityId, object component)
        {
            ComponentsByEntityId.Add(entityId, (T)component);
        }
        public void Remove(long entityId)
        {
            ComponentsByEntityId.Remove(entityId);
        }
    }
}
