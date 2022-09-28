using System;
using Microsoft.Xna.Framework;
using Ember.ECS.Components;
using Ember.Graphics;
using Ember.Tiles;

namespace Ember.ECS.Systems
{
    public class PlayerControllerSystem : System
    {
        private readonly Camera _camera;

        public PlayerControllerSystem(World world, Camera camera) : base(world, new Filter(world)
            .Include(typeof(Player)))
        {
            _camera = camera;
        }

        protected override void UpdateEntity(Entity entity, GameTime gameTime)
        {
            var player = entity.GetComponent<Player>();

            if (Input.MousePressed(MouseButton.Left))
            {
                Vector2 worldMousePosition = _camera.ScreenToWorld(Input.MousePosition);
                Vector2Int tilePosition = player.Tilemap.GetTileCoords(worldMousePosition);
                player.Tilemap.SetTile(tilePosition, TileId.Air);
                Console.WriteLine(tilePosition.X + " " + tilePosition.Y);
            }
            if (Input.MousePressed(MouseButton.Right))
            {
                Vector2 worldMousePosition = _camera.ScreenToWorld(Input.MousePosition);
                Vector2Int tilePosition = player.Tilemap.GetTileCoords(worldMousePosition);
                player.Tilemap.SetTile(tilePosition, TileId.Grass);
            }
        }
    }
}
