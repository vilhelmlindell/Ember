using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ember.Graphics;

namespace Ember.Items
{
    public class Item
    {
        public static readonly Item[] Items = new Item[ItemID.Count];

        public string Name;
        public int MaxCount;
        public Sprite Sprite;

        public static void LoadItems(ContentManager content)
        {
            Items[ItemID.Grass] = new Item()
            {
                Name = "Grass tile",
                MaxCount = 9999,
                Sprite = new Sprite(content.Load<Texture2D>("Assets/Sprites/Grass"))
            };
        }
    }
}
