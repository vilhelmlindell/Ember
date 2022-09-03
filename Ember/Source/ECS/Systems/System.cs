using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Ember.ECS.Systems
{
    public class System : ISystem
    {
        protected readonly World World;
        protected readonly Filter Filter;
        private readonly List<Entity> _activeEntities;
        private bool _rebuildActives;

        public System(World world, Filter filter)
        {
            World = world;
            Filter = filter;
            _activeEntities = new List<Entity>();
            _rebuildActives = true;

            World.EntityManager.EntityCreated += OnEntityCreated;
            World.EntityManager.EntityDestroyed += OnEntityDestroyed;
            World.EntityManager.EntityChanged += OnEntityChanged;
        }

        public bool IsEnabled { get; set; } = true;

        public List<Entity> ActiveEntities
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
        protected void OnEntityCreated(Entity entity)
        {
            if (Filter.MeetsRequirements(entity))
            {
                _activeEntities.Add(entity);
                OnEntityAdded(entity);
            }
            else
                OnEntityRemoved(entity);
        }
        protected void OnEntityDestroyed(Entity entity) => _rebuildActives = true;
        protected void OnEntityChanged(Entity entity) => _rebuildActives = true;

        protected void RebuildActives()
        {
            _activeEntities.Clear();

            foreach (var entity in World.EntityManager.Entities)
            {
                OnEntityCreated(entity);
            }

            _rebuildActives = false;
        }
    }
}
