using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Ember.Graphics;

namespace Ember.Tiles
{
    public enum TileType
    {
        Empty,
        Block,
        OneWay
    }

    public class Tile
    {
        public static Tile[] Tiles = new Tile[TileID.Count];

        public Sprite Sprite;
        public TileType TileType;

        public static void LoadTiles(ContentManager content)
        {
            Tiles[TileID.Air] = new Tile()
            {
                TileType = TileType.Empty
            };
            Tiles[TileID.Grass] = new Tile()
            {
                TileType = TileType.Block,
                Sprite = new Sprite(content.Load<Texture2D>("Assets/Sprites/Grass"))
            };
        }
    }
}
