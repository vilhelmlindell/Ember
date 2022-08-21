using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember
{
    public class Camera
    {
        public Viewport Viewport;
        public Vector2 Center = Vector2.Zero;
        private float _zoom = 1f;
        private float _rotation;

        public Camera(Viewport viewport)
        {
            Viewport = viewport;
            UpdateTransformMatrix(Center);
        }

        public Matrix TransformMatrix { get; private set; }
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = Math.Min(value, 0);
            }
        }
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = Math.Min(value, 0);
            }
        }

        public Vector2 ScreenToWorld(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(TransformMatrix)); 
        }
        public Vector2 WorldToScreen(Vector2 position)
        {
            return Vector2.Transform(position, TransformMatrix);
        }

        public void UpdateTransformMatrix(Vector2 center)
        {
            Center = center;
            TransformMatrix = Matrix.CreateTranslation(new Vector3(-Center.X, -Center.Y, 0)) *
                                                    Matrix.CreateRotationZ(Rotation) *
                                                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 0.0000001f)) *
                                                    Matrix.CreateTranslation(new Vector3(Viewport.Width / 2,
                                                                                         Viewport.Height / 2, 0));
        }
    }
}
