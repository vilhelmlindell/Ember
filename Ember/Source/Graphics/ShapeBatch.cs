using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Graphics
{
    public sealed class ShapeBatch
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly BasicEffect _effect;
        
        private bool _isDisposed;
        private VertexPositionColor[] _vertices;
        private int[] _indices;

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
            
            _effect?.Dispose();
            _isDisposed = true;
        }

        public void Begin()
        {
            if (_isStarted) throw new Exception("batching is already started.");

            Viewport viewport = _graphicsDevice.Viewport;
            _effect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, 0, viewport.Height, 0f, 1f);
            
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
                Console.WriteLine(_vertexCount);
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
    }
}
