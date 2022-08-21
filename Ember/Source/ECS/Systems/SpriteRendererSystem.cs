using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ember.ECS.Components;
using Ember.Graphics;

namespace Ember.ECS.Systems
{
    public class SpriteRendererSystem : System
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Camera _camera;

        public SpriteRendererSystem(World world, SpriteBatch spriteBatch, Camera camera) : base(world, new Filter(world)
            .Include(typeof(Position),
                     typeof(SpriteRenderer)))
        {
            _spriteBatch = spriteBatch;
            _camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate,
                               BlendState.AlphaBlend,
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
            var position = entity.GetComponent<Position>();
            var sprite = entity.GetComponent<SpriteRenderer>();

            _spriteBatch.Draw(sprite.Texture, 
                              position.Value + sprite.Offset, 
                              sprite.SourceRectangle, 
                              sprite.Color, 
                              sprite.Rotation,
                              sprite.Origin,
                              sprite.Scale, 
                              sprite.SpriteEffect, 
                              sprite.LayerDepth);
        }
    }
}
