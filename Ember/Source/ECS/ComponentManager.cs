using System;
using System.Collections.Generic;

namespace Ember.ECS
{
    public class ComponentManager
    {
        public readonly IComponentPool[] ComponentPools;
        private readonly Dictionary<Type, int> _componentTypesByComponentId;
        private readonly Dictionary<int, Type> _componentIDsByComponentType;
        private readonly World _world;

        public ComponentManager(World world)
        {
            _world = world;
            ComponentPools = new IComponentPool[world.MaxComponentTypes];
            _componentTypesByComponentId = new Dictionary<Type, int>();
            _componentIDsByComponentType = new Dictionary<int, Type>();
        }

        public T AddComponent<T>(Entity entity, T component)
        {
            if (_world.EntityManager.Entities[entity.Index].Id != entity.Id)
                return default(T);

            int componentId = GetComponentId<T>();
            if (!entity.ComponentBits[componentId])
            {
                ComponentPools[componentId].Add(entity.Id, component);
                entity.ComponentBits.Set(componentId, true);
                _world.EntityManager.EntityChanged?.Invoke(entity);
            }
            return component;
        }
        public T GetComponent<T>(Entity entity)
        {
            if (_world.EntityManager.Entities[entity.Index].Id != entity.Id)
                return default;

            int componentId = GetComponentId<T>();
            if (entity.ComponentBits[componentId])
            {
                return ((ComponentPool<T>)ComponentPools[componentId]).ComponentsByEntityId[entity.Id];
            }
            return default;
        }
        public void RemoveComponent<T>(Entity entity)
        {
            if (_world.EntityManager.Entities[entity.Index].Id != entity.Id)
                return;

            int componentId = GetComponentId<T>();
            if (entity.ComponentBits[componentId])
            {
                ComponentPools[componentId].Remove(entity.Id);
                entity.ComponentBits.Set(componentId, false);
                _world.EntityManager.EntityChanged?.Invoke(entity);
            }
        }
        public bool HasComponent<T>(Entity entity)
        {
            int componentId = GetComponentId<T>();
            return entity.ComponentBits[componentId];
        }
        public int GetComponentId<T>()
        {
            Type type = typeof(T);
            if (_componentTypesByComponentId.ContainsKey(type))
            {
                return _componentTypesByComponentId[type];
            }
            return RegisterComponentType<T>();
        }
        public int GetComponentId(Type type)
        {
            if (_componentTypesByComponentId.ContainsKey(type))
            {
                return _componentTypesByComponentId[type];
            }
            return RegisterComponentType(type);
        }

        private int RegisterComponentType<T>()
        {
            Type type = typeof(T);
            int componentId = _componentIDsByComponentType.Count;
            _componentTypesByComponentId.Add(type, componentId);
            _componentIDsByComponentType.Add(componentId, type);
            ComponentPools[componentId] = new ComponentPool<T>();
            return componentId;
        }
        private int RegisterComponentType(Type type)
        {
            int componentId = _componentIDsByComponentType.Count;
            _componentTypesByComponentId.Add(type, componentId);
            _componentIDsByComponentType.Add(componentId, type);
            ComponentPools[componentId] = (IComponentPool)Activator.CreateInstance(typeof(ComponentPool<>).MakeGenericType(type));
            return componentId;
        }
    }
}
