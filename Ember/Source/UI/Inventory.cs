﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ember.Graphics;
using Ember.Items;

namespace Ember.UI
{
    public sealed class Inventory : Element
    {
        public int Columns;
        public int Rows;

        private Image[] _itemFrames;
        private ItemSlot[] _itemSlots;
        private bool _isInventoryOpen;
        private int _selectedIndex = -1;

        public Inventory(UIManager uiManager, int columns, int rows, int elementWidth, int elementHeight,
            int columnMargin, int rowMargin, Sprite elementSprite) : base(uiManager)
        {
            Columns = Math.Max(columns, 1);
            Rows = Math.Max(rows, 1);

            _itemFrames = new Image[Columns * Rows];
            _itemSlots = new ItemSlot[Columns * Rows];

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    var image = new Image(uiManager, elementSprite);
                    image.X = column * (elementWidth + columnMargin);
                    image.Y = row * (elementHeight + rowMargin);
                    image.Width = elementWidth;
                    image.Height = elementHeight;
                    _itemFrames[row * Columns + column] = image;
                    ItemSlot itemSlot = new ItemSlot(uiManager);
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
            get => _isInventoryOpen;
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
                        _itemSlots[_selectedIndex].HorizontalAlignment = HorizontalAlignment.None;
                        _itemSlots[_selectedIndex].VerticalAlignment = VerticalAlignment.None;
                        _itemFrames[_selectedIndex].RemoveChild(_itemSlots[_selectedIndex]);
                    }
                }
            }

            if (_selectedIndex != -1)
            {
                _itemSlots[_selectedIndex].Position = Input.MousePosition;
                
                if (Input.MouseReleased(MouseButton.Left))
                {
                    for (int i = 0; i < _itemSlots.Length; i++)
                    {
                        if (_itemFrames[i].IsHovered)
                        {
                            if (_itemSlots[i].ItemStack == null)
                            {
                                ItemSlot itemSlot = _itemSlots[_selectedIndex]; 
                                _itemSlots = _itemSlots.Except(new ItemSlot[] { itemSlot }).ToArray();
                                _itemSlots[i] = itemSlot;
                            }
                            _itemFrames[i].AddChild(_itemSlots[i]);
                            _itemSlots[i].Position = Vector2.Zero;
                            _itemSlots[i].HorizontalAlignment = HorizontalAlignment.Center;
                            _itemSlots[i].VerticalAlignment = VerticalAlignment.Center;
                            _selectedIndex = -1;
                        }
                    }

                }
            }

            base.Update(gameTime);
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
