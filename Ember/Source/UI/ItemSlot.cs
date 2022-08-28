using Ember.Items;

namespace Ember.UI
{
    public class ItemSlot : Image
    {
        private TextBox _countTextBox = new TextBox();
        private ItemStack _itemStack;

        public ItemSlot()
        {
            _countTextBox.FontSize = 20;
            AddChild(_countTextBox);
        }

        public ItemStack ItemStack
        {
            get { return _itemStack; }
            set
            {
                _itemStack = value;
                Sprite = _itemStack.Item.Sprite;
                _countTextBox.Text = _itemStack.Count.ToString();
            }
        }
    }
}
