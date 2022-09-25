using Ember.Items;

namespace Ember.UI
{
    public class ItemSlot : Image
    {
        public TextBox CountTextBox;
        private ItemStack _itemStack;

        public ItemSlot(UIManager uiManager) : base(uiManager)
        {
            CountTextBox = new TextBox(uiManager);
            CountTextBox.FontSize = 20;
            AddChild(CountTextBox);
        }

        public ItemStack ItemStack
        {
            get => _itemStack;
            set
            {
                _itemStack = value;
                if (value != null)
                {
                    Sprite = _itemStack.Item.Sprite;
                    CountTextBox.Text = _itemStack.Count.ToString();
                }
                else
                {
                    Sprite = null;
                    CountTextBox.Text = "";
                }
            }
        }
    }
}
