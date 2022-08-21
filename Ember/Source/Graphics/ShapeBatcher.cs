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
            _game = game;

            const int MaxVertexCount = 1024;
            const int MaxIndexCount = MaxVertexCount * 3;

            _vertices = new VertexPositionColor[MaxVertexCount];
            _indices = new int[MaxIndexCount];
        }

        public void Begin()
        {
            _isStarted = true;
        }

        public void End()
        {
            _isStarted = false;
        }
    }
}
