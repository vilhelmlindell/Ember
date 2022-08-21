using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ember.Graphics;
using Ember.Items;

namespace Ember.GUI
{
    public class Inventory : Element
    {
        public int Columns;
        public int Rows;

        private ItemSlot[] _itemSlots;
        private bool _isInventoryOpen;
        private int _selectedIndex = -1;

        public Inventory(int columns, int rows, int elementWidth, int elementHeight, int columnMargin, int rowMargin, Sprite elementSprite)
        {
            Columns = Math.Max(columns, 1);
            Rows = Math.Max(rows, 1);

            _itemSlots = new ItemSlot[Columns * Rows];

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    Image image = new Image(elementSprite);
                    image.X = column * (elementWidth + columnMargin);
                    image.Y = row * (elementHeight + columnMargin);
                    image.Width = elementWidth;
                    image.Height = elementHeight;
                    ItemSlot itemSlot = new ItemSlot();
                    image.Children.Add(itemSlot.Image);
                    itemSlot.Image.Width = 16;
                    itemSlot.Image.Height = 16;
                    _itemSlots[row * Columns + column] = itemSlot;
                    Children.Add(image);
                }
            }
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
                    if (_itemSlots[i].Image == null) continue;

                    if (_itemSlots[i].Image.IsHovered)
                    {
                        Console.WriteLine("test");
                        _selectedIndex = i;
                    }
                }
            }

            if (_selectedIndex != -1)
            {
                _itemSlots[_selectedIndex].Image.Position = Input.MousePosition;
            }
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
