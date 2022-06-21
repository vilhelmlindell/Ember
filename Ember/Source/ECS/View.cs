using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ember.ECS
{
    public class View : IEnumerable
    {
        private World _world;
        private List<Entity> _entities;
        private List<Type> _with;
        private List<Type> _without;

        public View(World world)
        {
            _world = world;
            _entities = world.Entities;
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                yield return _entities[i];
            }
        }

        public View With(params Type[] types)
        {
            BitArray components = new BitArray(World.MaxComponents);
            for (int i = 0; i < types.Length; i++)
            {
                int componentId = (int)_world.GetType().GetMethod("GetComponentId").MakeGenericMethod(types[i]).Invoke(_world, null);
                components.Set(componentId, true);
            }
            List<Entity> entitiesToRemove = new List<Entity>();
            foreach (Entity entity in _entities)
            {
                BitArray temp = new BitArray(entity.ComponentMask);
                if (!temp.And(components).EqualTo(components))
                {
                    entitiesToRemove.Add(entity);
                }
            }
            _entities = _entities.Except(entitiesToRemove).ToList();
            return this;
        }
        public View Without(params Type[] types)
        {
            _with.AddRange(types);
            BitArray components = new BitArray(World.MaxComponents);
            for (int i = 0; i < types.Length; i++)
            {
                int componentId = (int)_world.GetType().GetMethod("GetComponentId").MakeGenericMethod(types[i]).Invoke(_world, null);
                components.Set(componentId, true);
            }
            List<Entity> entitiesToRemove = new List<Entity>();
            foreach (Entity entity in _entities)
            {
                BitArray temp = new BitArray(components);
                if (!temp.Not().Or(entity.ComponentMask).Not().EqualTo(components))
                {
                    entitiesToRemove.Add(entity);
                }
            }
            _entities = _entities.Except(entitiesToRemove).ToList();
            return this;
        }
    }
}

