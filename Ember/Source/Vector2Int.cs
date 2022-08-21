using Microsoft.Xna.Framework;

namespace Ember
{
    public class Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int() { }
        public Vector2Int(int x, int y) 
        { 
            X = x; 
            Y = y; 
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
    }
}
