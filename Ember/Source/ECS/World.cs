using System;
using System.Collections.Generic;
using Ember.Helpers;

namespace Ember.ECS
{

    public sealed class World
    {
        public const int MaxEntities = 1000;
        public const int MaxComponents = 32;

        private readonly List<int> _freeEntities;
        private readonly List<Type> _componentTypes;
        private readonly IComponentPool[] _componentPools;

        public World()
        {
            Entities = new List<Entity>();
            _freeEntities = new List<int>();
            _componentTypes = new List<Type>(MaxComponents);
            _componentPools = new IComponentPool[MaxComponents];
        }

        public Action<Entity> EntityCreated;
        public Action<Entity> EntityDestroyed;
        public Action<Entity> EntityChanged;

        public List<Entity> Entities { get; }

        public Entity CreateEntity()
        {
            if (!(_freeEntities.Count == 0))
            {
                int newIndex = _freeEntities[_freeEntities.Count - 1];
                _freeEntities.RemoveAt(newIndex);
                Entities[newIndex].Id = Entity.CreateId(newIndex, Entities[newIndex].Version);
                return Entities[newIndex];
            }
            Entity entity = new Entity(Entities.Count, 0);
            Entities.Add(entity);
            return entity;
        }
        public void DestroyEntity(Entity entity)
        {
            entity.Id = Entity.CreateId(-1, entity.Version + 1);
            entity.ComponentMask.SetAll(false);
            _freeEntities.Add(entity.Index);

            for (int i = 0; i < MaxComponents; i++)
            {
                if (entity.ComponentMask.Get(i))
                {
                    Type type = typeof(ComponentPool<>).MakeGenericType(_componentTypes[i]);
                    CastHelper.Cast(_componentPools[i], type).Components[entity.Index] = null;
                } 
            }
        }
        public void AddComponent<T>(Entity entity, T component)
        {
            if (Entities[entity.Index].Id != entity.Id)
                return;

            int componentId = GetComponentId<T>();
            (_componentPools[componentId] as ComponentPool<T>).Components[entity.Id] = component;
            entity.ComponentMask.Set(componentId, true);
        }
        public void AddComponent<T>(Entity entity)
        {
            if (Entities[entity.Index].Id != entity.Id)
                return;

            int componentId = GetComponentId<T>();
            (_componentPools[componentId] as ComponentPool<T>).Components[entity.Index] = (T)Activator.CreateInstance(typeof(T));
            entity.ComponentMask.Set(componentId, true);
        }
        public void RemoveComponent<T>(Entity entity)
        {
            if (Entities[entity.Index].Id != entity.Id)
                return;

            int componentId = GetComponentId<T>();
            (_componentPools[componentId] as ComponentPool<T>).Components[entity.Index] = default;
            entity.ComponentMask.Set(componentId, true);
        }
        public T GetComponent<T>(Entity entity)
        {
            if (Entities[entity.Index].Id != entity.Id)
                return default;

            int componentId = GetComponentId<T>();
            if (entity.ComponentMask[componentId])
            {
                return (_componentPools[componentId] as ComponentPool<T>).Components[entity.Index];
            }
            return default;
        }
        public int GetComponentId<T>()
        {
            int i = _componentTypes.IndexOf(typeof(T));
            if (i != -1)
            {
                return i;
            }
            _componentTypes.Add(typeof(T));
            int componentId = _componentTypes.Count - 1;
            _componentPools[componentId] = new ComponentPool<T>();
            return componentId;
        }
    }
}
