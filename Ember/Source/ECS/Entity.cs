using System.Collections;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("World")]

namespace Ember.ECS
{
    public class Entity
    {
        internal int Index;
        internal int Version;
        internal BitArray ComponentMask;

        internal Entity(int index, int version)
        {
            Index = index;
            Version = version;
            ComponentMask = new BitArray(World.MaxComponents);
        }

        public static long InvalidEntity = CreateId(-1, 0);
        public long Id
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

        internal static long CreateId(int index, int version)
        {
            return (index << 32) | version;
        }
    }
}
