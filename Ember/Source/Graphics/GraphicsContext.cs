using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Graphics
{
    /// <summary>
    /// Context class for accessing SpriteBatch and GraphicsDevice
    /// </summary>
    public class GraphicsContext
    {
        public readonly GraphicsDevice GraphicsDevice;
        public readonly SpriteBatch SpriteBatch;
        
        public GraphicsContext(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;
        }
        
        //private class SpriteDrawCall
        //{
        //    public Sprite Sprite;
        //    public Vector2 Position;
        //    public Vector2 Scale;
        //    public Vector2 Origin;
        //    public SpriteEffects SpriteEffect;
        //    public float Rotation;
        //    public float LayerDepth;
        //    public Effect Effect;

        //    public SpriteDrawCall(Sprite sprite)
        //    {
        //        Sprite = sprite;
        //    }

        //    public static int CompareByEffect(SpriteDrawCall x, SpriteDrawCall y)
        //    {
        //        return x.Effect.Equals(y.Effect) ? 0 : 1;
        //    }
        //}

        //private List<SpriteDrawCall> _spriteDrawCalls;


        //public void Clear(Color color)
        //{
        //    GraphicsDevice.Clear(color);
        //}
        //public void Begin(SpriteBatchArguments arguments)
        //{
        //    SpriteBatch.Begin(SpriteSortMode.Deferred,
        //                      arguments.BlendState,
        //                      arguments.SamplerState,
        //                      arguments.DepthStencilState,
        //                      arguments.RasterizerState,
        //                      arguments.Effect,
        //                      arguments.TransformMatrix);
        //}
        //public void DrawSprite(Sprite sprite, Vector2 position, Vector2 scale, Vector2 origin, float rotation,
        //                       float layerDepth, SpriteEffects spriteEffects, SpriteBatchArguments arguments)
        //{
        //}
        //public void End()
        //{
        //    _spriteDrawCalls.Sort(SpriteDrawCall.CompareByEffect);

        //    for (int i = 0; i < _spriteDrawCalls.Count; i++)
        //    {
        //    }
        //}
    }
}
