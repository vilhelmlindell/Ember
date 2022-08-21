using System;
using System.Collections.Generic;

namespace Ember.ECS
{
    public class EntityManager
    {
        public readonly List<Entity> Entities = new List<Entity>();
        private readonly List<int> _freeEntityIndices = new List<int>();
        private readonly World _world;

        public EntityManager(World world)
        {
            _world = world;
        }

        public Action<Entity> EntityCreated;
        public Action<Entity> EntityDestroyed;
        public Action<Entity> EntityChanged;

        public Entity CreateEntity()
        {
            if (_freeEntityIndices.Count != 0)
            {
                int newIndex = _freeEntityIndices[_freeEntityIndices.Count - 1];
                _freeEntityIndices.RemoveAt(newIndex);
                Entities[newIndex].ID = Entity.CreateId(newIndex, Entities[newIndex].Version);
                return Entities[newIndex];
            }
            Entity entity = new Entity(Entities.Count, 0, _world);
            Entities.Add(entity);
            EntityCreated?.Invoke(entity);
            return entity;
        }
        public void DestroyEntity(Entity entity)
        {
            entity.ID = Entity.CreateId(-1, entity.Version + 1);
            entity.ComponentBits.SetAll(false);
            _freeEntityIndices.Add(entity.Index);

            for (int i = 0; i < _world.MaxComponentTypes; i++)
            {
                if (entity.ComponentBits[i])
                {
                    _world.ComponentManager.ComponentPools[i].Remove(entity.ID);
                }
            }
            EntityDestroyed?.Invoke(entity);
        }
        public List<Entity> GetFilteredEntities(Filter filter)
        {
            List<Entity> entities = new List<Entity>();
            foreach (Entity entity in Entities)
            {
                if (filter.MeetsRequirements(entity))
                {
                    entities.Add(entity);
                }
            }
            return entities;
        }
    }
}
