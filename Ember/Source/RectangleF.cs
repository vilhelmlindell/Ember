using System;
using Microsoft.Xna.Framework;

namespace Ember
{
    public class RectangleF
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public RectangleF(Vector2 position, float width, float height)
        {
            Position = position;
            Width = width;
            Height = height;
        }
        public RectangleF(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Vector2 Size
        {
            get { return new Vector2(Width, Height); }
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }
        public float Left 
        { 
            get 
            { 
                return X; 
            } 
            set
            {
                X = value;
            }
        }
        public float Right
        {
            get
            {
                return X + Width;
            }
            set
            {
                X = value - Width;
            }
        }
        public float Top 
        { 
            get 
            { 
                return Y; 
            } 
            set
            {
                Y = value;
            }
        }
        public float Bottom 
        { 
            get 
            { 
                return Y + Height; 
            } 
            set
            {
                Y = value - Height;
            }
        }

        public Rectangle Rectangle => new Rectangle(Convert.ToInt32(X), Convert.ToInt32(Y),
                                                    Convert.ToInt32(Width), Convert.ToInt32(Height));

        public bool Contains(Vector2 position)
        {
            return position.X > X && position.X < X + Width &&
                   position.Y > Y && position.Y < Y + Height;
        }
        public bool Intersects(RectangleF other)
        {
            return (Left < other.Right) && (Right > other.Left) &&
                   (Top < other.Bottom) && (Bottom > other.Top);
        }
        public bool Intersects(Rectangle other)
        {
            return (Left < other.Right) && (Right > other.Left) &&
                   (Top < other.Bottom) && (Bottom > other.Top);
        }
    }
}
