using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ember.ECS.Components;
using Ember.Tiles;
using Ember.Graphics;

namespace Ember.ECS.Systems
{
    public class TilemapRendererSystem : System
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Camera _camera;

        public TilemapRendererSystem(World world, SpriteBatch spriteBatch, Camera camera) : base(world, new Filter(world)
            .Include(typeof(Tilemap)))
        {
            _spriteBatch = spriteBatch;
            _camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate,
                               null,
                               SamplerState.PointClamp,
                               DepthStencilState.Default,
                               null,
                               Shaders.ClipTransparent,
                               _camera.TransformMatrix);
            base.Update(gameTime);
            _spriteBatch.End();
        }

        protected override void UpdateEntity(Entity entity, GameTime gameTime)
        {
            var tilemap = entity.GetComponent<Tilemap>();

            for (int x = 0; x < tilemap.Width; x++)
            {
                for (int y = 0; y < tilemap.Height; y++)
                {
                    int tileID = tilemap.GetTile(x, y);
                    Sprite sprite = Tile.Tiles[tileID].Sprite;
                    if (sprite != null)
                    {
                        Vector2 tilePosition = new Vector2(x * tilemap.TileWidth, y * tilemap.TileHeight);
                        _spriteBatch.Draw(sprite.Texture, 
                                          tilePosition, 
                                          sprite.SourceRectangle, 
                                          Color.White, 
                                          0f, 
                                          Vector2.Zero, 
                                          new Vector2(2, 2), 
                                          SpriteEffects.None, 
                                          DrawLayer.Default);
                    }
                }
            }
        }
    }
}
