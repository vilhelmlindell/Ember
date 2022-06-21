namespace Ember.ECS
{
    public interface IComponentPool 
    {
    }

    public class ComponentPool<T> : IComponentPool
    {
        public T[] Components;

        public ComponentPool()
        {
            Components = new T[World.MaxEntities];
        }
    }
}
