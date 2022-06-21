using System;

namespace Ember.Helpers
{
    public static class CastHelper
    {
        public static dynamic Cast(object src, Type t)
        {
            var castMethod = typeof(CastHelper).GetMethod("CastGeneric").MakeGenericMethod(t);
            return castMethod.Invoke(null, new[] { src });
        }
        public static T CastGeneric<T>(object src)
        {
            return (T)Convert.ChangeType(src, typeof(T));
        }
    }
}
