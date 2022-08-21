using Ember.Items;

namespace Ember.GUI
{
    public class ItemSlot
    {
        public Image Image = new Image();
        public TextBox TextBox = new TextBox();
        private ItemStack _itemStack;

        public ItemSlot()
        {
            TextBox.FontSize = 20;
            Image.Children.Add(TextBox);
        }

        public ItemStack ItemStack
        {
            get { return _itemStack; }
            set
            {
                _itemStack = value;
                Image.Sprite = _itemStack.Item.Sprite;
                TextBox.Text = _itemStack.Count.ToString();
            }
        }
    }
}
