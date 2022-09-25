using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Graphics
{
    public sealed class ShapeBatcher
    {
        private Game _game;

        private VertexPositionColor[] _vertices;
        private int[] _indices;

        private int _shapeCount;
        private int _vertexCount;
        private int _indexCount;

        private bool _isStarted;

        public ShapeBatcher(Game game)
        {
            _game = game ?? throw new ArgumentNullException("game");

            const int MaxVertexCount = 1024;
            const int MaxIndexCount = MaxVertexCount * 3;

            _vertices = new VertexPositionColor[MaxVertexCount];
            _indices = new int[MaxIndexCount];
        }

        public void Begin()
        {
            if (_isStarted)
            {
                throw new Exception("batching is already started.");
            }
            _isStarted = true;
        }

        public void End()
        {
            if (_isStarted)
            {
                throw new Exception("batching was never started.");
            }
            Flush();
            _isStarted = false;
        }

        public void Flush()
        {
            if (!_isStarted)
            {
                throw new Exception("batching was never started.");
            }
        }

        public void EnsureStarted()
        {
            if (!_isStarted)
            {
                throw new Exception("batching was never started.");
            }
        }

        public void EnsureSpace(int shapeVertexCount, int shapeIndexCount)
        {
            if (shapeVertexCount > _vertices.Length)
            {
                throw new Exception("Maximum shape vertex count is: " + _vertices.Length);
            }
            
            if (shapeIndexCount > _indices.Length)
            {
                throw new Exception("Maximum shape index count is: " + _indices.Length);
            }
            
            if (_vertexCount + shapeVertexCount > _vertices.Length ||
                _indexCount + shapeIndexCount > _indices.Length)
            {
                Flush();
            }
        }

        public void DrawRectangle(float x, float y, float width, float height, Color color)
        {
            EnsureStarted();
        }
    }
}
