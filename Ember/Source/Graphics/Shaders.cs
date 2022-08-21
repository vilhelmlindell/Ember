using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Ember.Graphics
{
    public static class Shaders
    {
        public static Effect ClipTransparent;

        public static void LoadShaders(ContentManager content, GraphicsDevice graphicsDevice)
        {
            ClipTransparent = content.Load<Effect>("Assets/Shaders/Effect");
        }
    }
}
