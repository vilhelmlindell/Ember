using System.Collections;
namespace Ember.ECS
{
    public class Entity
    {
        public readonly World World;

        public Entity(int index, int version, World world)
        {
            World = world;
            Index = index;
            Version = version;
            ComponentBits = new BitArray(world.MaxComponentTypes);
        }
        private Entity(long id)
        {
            ID = id;
        }

        public long ID
        {
            get 
            { 
                return CreateId(Index, Version); 
            }
            set
            {
                Index = (int)(value >> 32);
                Version = (int)value;
            }
        }
        public int Index { get; internal set; }
        public int Version { get; internal set; }
        public BitArray ComponentBits { get; internal set; }

        public static long InvalidId = CreateId(-1, 0);
        public static Entity Invalid = new Entity(InvalidId);
        public static long CreateId(int index, int version)
        {
            return (index << 32) | version;
        }

        public void AddComponent<T>(T component)
        {
            World.ComponentManager.AddComponent(this, component);
        }
        public void RemoveComponent<T>()
        {
            World.ComponentManager.RemoveComponent<T>(this);
        }
        public T GetComponent<T>()
        {
            return World.ComponentManager.GetComponent<T>(this);
        }
        public bool HasComponent<T>()
        {
            return World.ComponentManager.HasComponent<T>(this);
        }    
    }
}
