using System;
using Microsoft.Xna.Framework;
using Ember.Utilities;

namespace Ember.Easing
{
    public static class Easing
    {
        public static float Ease(Func<float, float> function, float startValue, float targetValue, float time)
        {
            return function(time) * (targetValue - startValue);
        }
        public static RectangleF Ease(Func<float, float> function, RectangleF startValue, RectangleF targetValue, float time)
        {
            return new RectangleF(Ease(function, startValue.X, targetValue.X, time),
                                  Ease(function, startValue.Y, targetValue.Y, time),
                                  Ease(function, startValue.Width, targetValue.Width, time),
                                  Ease(function, startValue.Height, targetValue.Height, time));
        }

        public static float Flip(float t)
        {
            return 1 - t;
        }
        public static float Scale(float a, float t)
        {
            return a * t;
        }
        public static float ReverseScale(float a, float t)
        {
            return (1 - a) * t;
        }
        public static float Mix(float x, float y, float weightY)
        {
            return x + weightY * (y - x);
        }
        public static float BounceClampBottom(float t)
        {
            return Math.Abs(t);
        }
        public static float BounceClampTop(float t)
        {
            return 1 - Math.Abs(1 - t);
        }
        public static float BounceClampBottomTop(float t)
        {
            return BounceClampTop(BounceClampBottom(t));
        }
    }
}
