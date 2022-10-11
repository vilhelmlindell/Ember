using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Graphics
{
    public sealed class ShapeBatch
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly BasicEffect _effect;
        
        private readonly VertexPositionColor[] _vertices;
        private readonly int[] _indices;
        private bool _isDisposed;

        private int _shapeCount;
        private int _vertexCount;
        private int _indexCount;

        private bool _isStarted;

        public ShapeBatch(GraphicsDevice graphicsDevice)
        {
            _isDisposed = false;
            _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));

            _effect = new BasicEffect(graphicsDevice);
            _effect.TextureEnabled = false;
            _effect.FogEnabled = false;
            _effect.LightingEnabled = false;
            _effect.VertexColorEnabled = true;
            _effect.World = Matrix.Identity;
            _effect.View = Matrix.Identity;
            _effect.Projection = Matrix.Identity;

            const int maxVertexCount = 1024;
            const int maxIndexCount = maxVertexCount * 3;

            _vertices = new VertexPositionColor[maxVertexCount];
            _indices = new int[maxIndexCount];
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            
            _effect.Dispose();
            _isDisposed = true;
        }

        public void Begin()
        {
            if (_isStarted) throw new Exception("batching is already started.");

            Viewport viewport = _graphicsDevice.Viewport;
            _effect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0f, 1f);
            
            _isStarted = true;
        }

        public void End()
        {
            if (!_isStarted) throw new Exception("batching was never started.");
            
            Flush();
            _isStarted = false;
        }

        public void Flush()
        {
            if (_shapeCount == 0) return;
            
            EnsureStarted();

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList,
                    _vertices,
                    0,
                    _vertexCount,
                    _indices,
                    0,
                    _indexCount / 3);
            }

            _shapeCount = 0;
            _vertexCount = 0;
            _indexCount = 0;
        }

        public void EnsureStarted()
        {
            if (!_isStarted) throw new Exception("batching was never started.");
        }

        public void EnsureSpace(int shapeVertexCount, int shapeIndexCount)
        {
            if (shapeVertexCount > _vertices.Length) throw new Exception("Maximum shape vertex count is: " + _vertices.Length);
            
            if (shapeIndexCount > _indices.Length) throw new Exception("Maximum shape index count is: " + _indices.Length);
            
            if (_vertexCount + shapeVertexCount > _vertices.Length ||
                _indexCount + shapeIndexCount > _indices.Length)
            {
                Flush();
            }
        }

        public void DrawRectangle(float x, float y, float width, float height, Color color)
        {
            EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;
            
            EnsureSpace(shapeVertexCount, shapeIndexCount);

            float left = x;
            float right = x + width;
            float top = y;
            float bottom = y + height;

            Vector2 a = new Vector2(left, top);
            Vector2 b = new Vector2(right, top);
            Vector2 c = new Vector2(right, bottom);
            Vector2 d = new Vector2(left, bottom);

            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;

            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(a, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(b, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(c, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(d, 0f), color);

            _shapeCount++;
        }
        public void DrawRectangle(Vector2 position, Vector2 size, Color color)
        {
            EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;
            
            EnsureSpace(shapeVertexCount, shapeIndexCount);

            float left = position.X;
            float right = position.X + size.X;
            float top = position.Y;
            float bottom = position.Y + size.Y;

            Vector2 a = new Vector2(left, top);
            Vector2 b = new Vector2(right, top);
            Vector2 c = new Vector2(right, bottom);
            Vector2 d = new Vector2(left, bottom);

            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;

            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(a, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(b, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(c, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(d, 0f), color);

            _shapeCount++;
        }

        public void DrawLine(float x1, float y1, float x2, float y2, float thickness, Color color)
        {
            EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;
            
            EnsureSpace(shapeVertexCount, shapeIndexCount);

            float k1 = ((y2 - y1) / (x2 - x1));
            float k2 = -1 / k1;
            float xt = MathF.Sqrt((thickness * thickness) / (4 * (1 + k2 * k2)));
            float yt = k2 * xt;

            Vector2 q1 = new Vector2(x1 + xt, y1 + yt);
            Vector2 q2 = new Vector2(x2 + xt, y2 + yt);
            Vector2 q3 = new Vector2(x2 - xt, y2 - yt);
            Vector2 q4 = new Vector2(x1 - xt, y1 - yt);

            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;

            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q1, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q2, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q3, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q4, 0f), color);

            _shapeCount++;
        }
        public void DrawLine(Vector2 p1, Vector2 p2, float thickness, Color color)
        {
            EnsureStarted();

            const int shapeVertexCount = 4;
            const int shapeIndexCount = 6;
            
            EnsureSpace(shapeVertexCount, shapeIndexCount);

            float k1 = ((p2.Y - p1.Y) / (p2.X - p1.X));
            float k2 = -1 / k1;
            float xt = MathF.Sqrt((thickness * thickness) / (4 * (1 + k2 * k2)));
            float yt = k2 * xt;

            Vector2 q1 = new Vector2(p1.X + xt, p1.Y + yt);
            Vector2 q2 = new Vector2(p2.X + xt, p2.Y + yt);
            Vector2 q3 = new Vector2(p2.X - xt, p2.Y - yt);
            Vector2 q4 = new Vector2(p1.X - xt, p1.Y - yt);

            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 1 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 0 + _vertexCount;
            _indices[_indexCount++] = 2 + _vertexCount;
            _indices[_indexCount++] = 3 + _vertexCount;

            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q1, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q2, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q3, 0f), color);
            _vertices[_vertexCount++] = new VertexPositionColor(new Vector3(q4, 0f), color);

            _shapeCount++;
        }
    }
}
