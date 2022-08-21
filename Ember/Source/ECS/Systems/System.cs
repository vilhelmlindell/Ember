using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Ember.ECS.Systems
{
    public class System : ISystem
    {
        protected World _world;
        protected Filter _filter;
        protected readonly List<Entity> _activeEntities;
        protected bool _rebuildActives;

        public System(World world, Filter filter)
        {
            _world = world;
            _filter = filter;
            _activeEntities = new List<Entity>();
            _rebuildActives = true;

            _world.EntityManager.EntityCreated += OnEntityCreated;
            _world.EntityManager.EntityDestroyed += OnEntityDestroyed;
            _world.EntityManager.EntityChanged += OnEntityChanged;
        }

        public virtual bool IsEnabled { get; set; } = true;

        public virtual List<Entity> ActiveEntities
        {
            get
            {
                if (_rebuildActives)
                    RebuildActives();

                return _activeEntities;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsEnabled)
            {
                foreach (Entity entity in ActiveEntities)
                {
                    UpdateEntity(entity, gameTime);
                }
            }
        }
        protected virtual void UpdateEntity(Entity entity, GameTime gameTime) { }

        protected void OnEntityAdded(Entity entity) { }
        protected void OnEntityRemoved(Entity entity) { }
        protected virtual void OnEntityCreated(Entity entity)
        {
            if (_filter.MeetsRequirements(entity))
            {
                _activeEntities.Add(entity);
                OnEntityAdded(entity);
            }
            else
                OnEntityRemoved(entity);
        }
        protected virtual void OnEntityDestroyed(Entity entity) => _rebuildActives = true;
        protected virtual void OnEntityChanged(Entity entity) => _rebuildActives = true;

        protected virtual void RebuildActives()
        {
            _activeEntities.Clear();

            foreach (Entity entity in _world.EntityManager.Entities)
            {
                OnEntityCreated(entity);
            }

            _rebuildActives = false;
        }
    }
}
