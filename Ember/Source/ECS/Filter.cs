using System;
using System.Collections;
using Ember.Extensions;

namespace Ember.ECS
{
    public class Filter
    {
        private readonly World _world;
        private readonly BitArray _includedComponentBits;
        private readonly BitArray _excludedComponentBits;
        private bool _includedBitsSet;
        private bool _excludedBitsSet;

        public Filter(World world)
        {
            _world = world;
            _includedComponentBits = new BitArray(world.MaxComponentTypes);
            _excludedComponentBits = new BitArray(world.MaxComponentTypes);
        }

        public Filter Include(params Type[] componentTypes)
        {
            if (!_includedBitsSet)
            {
                foreach (Type type in componentTypes)
                {
                    int componentId = _world.ComponentManager.GetComponentID(type);
                    _includedComponentBits[componentId] = true;
                }
                _includedBitsSet = true;
            }
            return this;
        }
        public Filter Exclude(params Type[] componentTypes)
        {
            if (!_excludedBitsSet)
            {
                foreach (Type type in componentTypes)
                {
                    int componentId = _world.ComponentManager.GetComponentID(type);
                    _excludedComponentBits[componentId] = true;
                }
                _excludedBitsSet = false;
            }
            return this;
        }
        public bool MeetsRequirements(Entity entity)
        {
            return _includedComponentBits.UnmodifiedAnd(entity.ComponentBits).EqualBits(_includedComponentBits) &&
                   _excludedComponentBits.UnmodifiedNot().UnmodifiedOr(entity.ComponentBits).UnmodifiedNot().EqualBits(_excludedComponentBits);
        }
    }
}
