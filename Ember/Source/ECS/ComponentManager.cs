using System;
using System.Collections.Generic;

namespace Ember.ECS
{
    public class ComponentManager
    {
        public readonly IComponentPool[] ComponentPools;
        private readonly Dictionary<Type, int> _componentTypesByComponentID;
        private readonly Dictionary<int, Type> _componentIDsByComponentType;
        private readonly World _world;

        public ComponentManager(World world)
        {
            _world = world;
            ComponentPools = new IComponentPool[world.MaxComponentTypes];
            _componentTypesByComponentID = new Dictionary<Type, int>();
            _componentIDsByComponentType = new Dictionary<int, Type>();
        }

        public T AddComponent<T>(Entity entity, T component)
        {
            if (_world.EntityManager.Entities[entity.Index].ID != entity.ID)
                return default(T);

            int componentID = GetComponentID<T>();
            if (!entity.ComponentBits[componentID])
            {
                ComponentPools[componentID].Add(entity.ID, component);
                entity.ComponentBits.Set(componentID, true);
                _world.EntityManager.EntityChanged?.Invoke(entity);
            }
            return component;
        }
        public T GetComponent<T>(Entity entity)
        {
            if (_world.EntityManager.Entities[entity.Index].ID != entity.ID)
                return default;

            int componentID = GetComponentID<T>();
            if (entity.ComponentBits[componentID])
            {
                return ((ComponentPool<T>)ComponentPools[componentID]).ComponentsByEntityID[entity.ID];
            }
            return default;
        }
        public void RemoveComponent<T>(Entity entity)
        {
            if (_world.EntityManager.Entities[entity.Index].ID != entity.ID)
                return;

            int componentID = GetComponentID<T>();
            if (entity.ComponentBits[componentID])
            {
                ComponentPools[componentID].Remove(entity.ID);
                entity.ComponentBits.Set(componentID, false);
                _world.EntityManager.EntityChanged?.Invoke(entity);
            }
        }
        public bool HasComponent<T>(Entity entity)
        {
            int componentID = GetComponentID<T>();
            return entity.ComponentBits[componentID];
        }
        public int GetComponentID<T>()
        {
            Type type = typeof(T);
            if (_componentTypesByComponentID.ContainsKey(type))
            {
                return _componentTypesByComponentID[type];
            }
            return RegisterComponentType<T>();
        }
        public int GetComponentID(Type type)
        {
            if (_componentTypesByComponentID.ContainsKey(type))
            {
                return _componentTypesByComponentID[type];
            }
            return RegisterComponentType(type);
        }

        private int RegisterComponentType<T>()
        {
            Type type = typeof(T);
            int componentID = _componentIDsByComponentType.Count;
            _componentTypesByComponentID.Add(type, componentID);
            _componentIDsByComponentType.Add(componentID, type);
            ComponentPools[componentID] = new ComponentPool<T>();
            return componentID;
        }
        private int RegisterComponentType(Type type)
        {
            int componentID = _componentIDsByComponentType.Count;
            _componentTypesByComponentID.Add(type, componentID);
            _componentIDsByComponentType.Add(componentID, type);
            ComponentPools[componentID] = (IComponentPool)Activator.CreateInstance(typeof(ComponentPool<>).MakeGenericType(type));
            return componentID;
        }
    }
}
