using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Ember.ECS.Components;
using Ember.Extensions;
using Ember.Utilities;

namespace Ember.ECS.Systems
{
    public class TilemapCollisionSystem : System
    {
        private bool _updateHorizontal = true;

        public TilemapCollisionSystem(World world) : base(world, new Filter(world)
            .Include(typeof(Position),
                     typeof(Physics),
                     typeof(BoxCollider)))
        {
        }

        protected override void UpdateEntity(Entity entity, GameTime gameTime)
        {
            if (_updateHorizontal)
            {
                CheckHorizontalCollisions(entity);
            }
            else
            {
                CheckVerticalCollisions(entity);
            }
        }

        private void CheckHorizontalCollisions(Entity entity)
        {
            var position = entity.GetComponent<Position>();
            var collider = entity.GetComponent<BoxCollider>();
            var physics = entity.GetComponent<Physics>();

            collider.TouchingRight = false;
            collider.TouchingLeft = false;
            foreach (Rectangle other in GetCollisions(position, collider))
            {
                if (physics.Velocity.X > 0) // Moving right
                {
                    position.Value.X = other.Left - Convert.ToInt32(collider.Width);
                    collider.TouchingRight = true;
                }
                else if (physics.Velocity.X < 0) // Moving left
                {
                    position.Value.X = other.Right;
                    collider.TouchingLeft = true;    
                }
                physics.Velocity.X = 0;
            }

            _updateHorizontal = !_updateHorizontal; 
        }
        private void CheckVerticalCollisions(Entity entity)
        {
            var position = entity.GetComponent<Position>();
            var collider = entity.GetComponent<BoxCollider>();
            var physics = entity.GetComponent<Physics>();

            collider.TouchingDown = false;
            collider.TouchingUp = false;
            foreach (Rectangle other in GetCollisions(position, collider))
            {
                if (physics.Velocity.Y > 0) // Moving down
                {
                    position.Value.Y = other.Top - Convert.ToInt32(collider.Height);
                    collider.TouchingDown = true;
                }
                else if (physics.Velocity.Y < 0) // Moving up
                {
                    position.Value.Y = other.Bottom;
                    collider.TouchingUp = true;
                }
                physics.Velocity.Y = 0;
            }

            _updateHorizontal = !_updateHorizontal;
        }
        private List<Rectangle> GetCollisions(Position position, BoxCollider collider)
        {
            var tilemap = collider.Tilemap;

            RectangleF bounds = new RectangleF(position.Value, collider.Width, collider.Height);
            List<Rectangle> collisions = new List<Rectangle>();

            int leftTile = (int)Math.Floor(bounds.Left / tilemap.TileWidth);
            int rightTile = (int)Math.Ceiling(bounds.Right / tilemap.TileWidth) - 1;
            int topTile = (int)Math.Floor(bounds.Top / tilemap.TileHeight);
            int bottomTile = (int)Math.Ceiling(bounds.Bottom / tilemap.TileHeight) - 1;

            for (int y = topTile; y <= bottomTile; y++)
            {
                for (int x = leftTile; x <= rightTile; x++)
                {
                    if (tilemap.IsObstacle(x, y))
                    {
                        Rectangle tile = new Rectangle(x * tilemap.TileWidth, y * tilemap.TileHeight, tilemap.TileWidth, tilemap.TileHeight);

                        if (bounds.Intersects(tile))
                        {
                            collisions.Add(tile);
                        }
                    }
                }
            }
            return collisions;
        }
    }
}
