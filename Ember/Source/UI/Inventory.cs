using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ember.Graphics;
using Ember.Items;

namespace Ember.UI
{
    public class Inventory : Element
    {
        public int Columns;
        public int Rows;

        private Image[] _itemFrames;
        private ItemSlot[] _itemSlots;
        private bool _isInventoryOpen;
        private int _selectedIndex = -1;

        public Inventory(int columns, int rows, int elementWidth, int elementHeight, int columnMargin, int rowMargin, Sprite elementSprite)
        {
            Columns = Math.Max(columns, 1);
            Rows = Math.Max(rows, 1);

            _itemFrames = new Image[Columns * Rows];
            _itemSlots = new ItemSlot[Columns * Rows];

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    Image image = new Image(elementSprite);
                    image.X = column * (elementWidth + columnMargin);
                    image.Y = row * (elementHeight + rowMargin);
                    image.Width = elementWidth;
                    image.Height = elementHeight;
                    _itemFrames[row * Columns + column] = image;
                    ItemSlot itemSlot = new ItemSlot();
                    itemSlot.HorizontalAlignment = HorizontalAlignment.Center;
                    itemSlot.VerticalAlignment = VerticalAlignment.Center;
                    itemSlot.Width = 16;
                    itemSlot.Height = 16;
                    _itemSlots[row * Columns + column] = itemSlot;
                    itemSlot.LayerDepth = DrawLayer.UI + DrawLayer.Increment * 2;
                    image.AddChild(itemSlot);
                    AddChild(image);
                }
            }

            Width = Columns * (elementWidth + columnMargin - 1);
            Height = Rows * (elementHeight + rowMargin - 1);
        }

        public bool IsInventoryOpen
        {
            get { return _isInventoryOpen; }
            set
            {
                if (_isInventoryOpen != value)
                    ToggleInventory();
                _isInventoryOpen = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.KeyPressed(Keys.Escape))
                ToggleInventory();

            if (Input.MousePressed(MouseButton.Left))
            {
                for (int i = 0; i < _itemSlots.Length; i++)
                {
                    if (_itemFrames[i].IsHovered && _itemSlots[i].ItemStack != null)
                    {
                        _selectedIndex = i;
                    }
                }
            }

            if (_selectedIndex != -1)
            {
                if (_itemSlots[_selectedIndex].Sprite == null) return;

                _itemSlots[_selectedIndex].Position = Input.MousePosition;
            }
            base.Update(gameTime);
        }

        public void AddItemStack(ItemStack itemStack, int column, int row)
        {
        }
        public void AddItemStack(ItemStack itemStack, int index)
        {
            index = Math.Clamp(index, 0, Columns * Rows);

            if (_itemSlots[index].ItemStack != null) return;

            _itemSlots[index].ItemStack = itemStack;
        }
        public void ToggleInventory()
        {
            _isInventoryOpen = !_isInventoryOpen;
            for (int row = 1; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    Children[row * Columns + column].IsEnabled = !Children[row * Columns + column].IsEnabled;
                }
            }
        }
    }
}
