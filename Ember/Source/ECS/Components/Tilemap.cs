using System;
using Microsoft.Xna.Framework;
using Ember.Tiles;

namespace Ember.ECS.Components
{
    public class Tilemap
    {
        public readonly int TileWidth;
        public readonly int TileHeight;
        private readonly int[,] _tiles;

        public Tilemap(int width, int height, int tileWidth, int tileHeight)
        {
            _tiles = new int[width, height];
            TileWidth = width;
            TileHeight = height;
        }
        public Tilemap(int width, int height, int tileSize)
        {
            _tiles = new int[width, height];
            TileWidth = tileSize;
            TileHeight = tileSize;
        }

        public int Width => _tiles.GetLength(0);
        public int Height => _tiles.GetLength(1);

        public int GetTile(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                return _tiles[x, y];
            else
                return 0;
        }
        public int GetTile(Vector2Int tilePosition)
        {
            return GetTile(tilePosition.X, tilePosition.Y);
        }
        public Vector2Int GetTileCoords(Vector2 tilePosition)
        {
            return new Vector2Int((int)Math.Floor(tilePosition.X / TileWidth), (int)Math.Floor(tilePosition.Y / TileHeight));
        }
        public bool IsObstacle(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return true;
            return Tile.Tiles[_tiles[x, y]].TileType == TileType.Block;
        }
        public bool IsObstacle(Vector2Int tilePosition)
        {
            return IsObstacle(tilePosition.X, tilePosition.Y);
        }
        public void SetTile(int x, int y, int tileId)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                _tiles[x, y] = tileId;
        }
        public void SetTile(Vector2Int tilePosition, int tileId)
        {
            SetTile(tilePosition.X, tilePosition.Y, tileId);
        }
    }
}
