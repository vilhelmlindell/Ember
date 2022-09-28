using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ember.Graphics;

namespace Ember.UI
{
    public enum HorizontalAlignment
    {
        Left,
        Right,
        Center
    }
    public enum VerticalAlignment
    {
        Top,
        Bottom,
        Center
    }
    public enum LengthUnit
    {
        Pixels,
        Percent
    }

    /// <summary>
    /// Describes the layout of a control
    /// </summary>
    public class Layout
    {
        private float _x;
        private float _y;
        private float _width;
        private float _height;
        private LengthUnit _xUnit = LengthUnit.Pixels;
        private LengthUnit _yUnit = LengthUnit.Pixels;
        private LengthUnit _widthUnit = LengthUnit.Pixels;
        private LengthUnit _heightUnit = LengthUnit.Pixels;
        private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Left;
        private VerticalAlignment _verticalAlignment = VerticalAlignment.Top;


        public float X
        {
            get => _x;
            set { _x = value; LayoutChange?.Invoke(); }
        }
        public float Y
        {
            get => _y;
            set { _y = value; LayoutChange?.Invoke(); }
        }
        public float Width
        {
            get => _width;
            set { _width = value; LayoutChange?.Invoke(); }
        }
        public float Height
        {
            get => _height;
            set { _height = value; LayoutChange?.Invoke(); }
        }
        public LengthUnit XUnit
        {
            get => _xUnit;
            set { _xUnit = value; LayoutChange?.Invoke(); }
        }
        public LengthUnit YUnit
        {
            get => _yUnit;
            set { _yUnit = value; LayoutChange?.Invoke(); }
        }
        public LengthUnit WidthUnit
        {
            get => _widthUnit;
            set { _widthUnit = value; LayoutChange?.Invoke(); }
        }
        public LengthUnit HeightUnit
        {
            get => _heightUnit;
            set { _heightUnit = value; LayoutChange?.Invoke(); }
        }
        public HorizontalAlignment HorizontalAlignment
        {
            get => _horizontalAlignment;
            set { _horizontalAlignment = value; LayoutChange?.Invoke(); }
        }
        public VerticalAlignment VerticalAlignment
        {
            get => _verticalAlignment;
            set { _verticalAlignment = value; LayoutChange?.Invoke(); }
        }

        public Action LayoutChange;
    }

    /// <summary>
    /// Base ui element
    /// </summary>
    public class Control
    {
        public readonly Layout Layout;
        public List<Control> Children = new List<Control>();
        public bool IsHovered;
        public bool IsHeld;
        
        private readonly UiManager _uiManager;
        private readonly List<Control> _childrenToAdd = new List<Control>();
        private readonly List<Control> _childrenToRemove = new List<Control>();
        private float _x;
        private float _y;
        private float _width;
        private float _height;
        private float _layerDepth = DrawLayer.Ui;
        
        
        public Control()
        {
            Layout = new Layout();
            Layout.LayoutChange += CalculateLayout;
            Moved += OnMove;
            Resized += OnResize;
            Hovered += OnHover;
            Unhovered += OnUnhover;
            Pressed += OnPress;
            Released += OnRelease;
        }
        public Control(Layout layout)
        {
            Layout = layout;
            layout.LayoutChange += CalculateLayout;
            Moved += OnMove;
            Resized += OnResize;
            Hovered += OnHover;
            Unhovered += OnUnhover;
            Pressed += OnPress;
            Released += OnRelease;
        }

        public bool IsEnabled { get; set; } = true;
        public float X
        {
            get => _x;
            private set
            {
                _x = value;
                Moved?.Invoke();
            }
        }
        public float Y
        {
            get => _y;
            private set
            {
                _y = value;
                Moved?.Invoke();
            }
        }
        public float Width
        {
            get => _width;
            private set
            {
                _width = value;
                Resized?.Invoke();
            }
        }
        public float Height
        {
            get => _height;
            private set
            {
                _height = value;
                Resized?.Invoke();
            }
        }
        public float LayerDepth
        {
            get => _layerDepth;
            set
            {
                _layerDepth = value;
                foreach (Control element in Children)
                    if (element.LayerDepth <= LayerDepth)
                        element.LayerDepth = LayerDepth + DrawLayer.Increment;
            }
        }
        public Vector2 Position
        {
            get => new (X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Vector2 AbsolutePosition
        {
            get
            {
                if (Parent != null)
                    return Parent.AbsolutePosition + Position;
                return Position;
            }
        }
        public Vector2 Size
        {
            get => new(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }
        public RectangleF Bounds
        {
            get => new RectangleF(Position, Size);
            set
            {
                Position = new Vector2(value.X, value.Y);
                Size = new Vector2(value.Width, value.Height);
            }
        }
        public RectangleF AbsoluteBounds => new RectangleF(AbsolutePosition, Size);
        public Control Parent { get; private set; }

        public Action Moved;
        public Action Resized;
        public Action Hovered;
        public Action Unhovered;
        public Action Pressed;
        public Action Released;
        
        public void Update(GameTime gameTime)
        {
            if (IsEnabled)
            {
                OnUpdate(gameTime);
                foreach (Control child in Children)
                {
                    if (child.IsEnabled)
                        child.Update(gameTime);
                }
            }
            Children = Children.Except(_childrenToRemove).ToList();
            Children.AddRange(_childrenToAdd);
            foreach (Control child in _childrenToAdd)
            {
                if (child.LayerDepth <= LayerDepth)
                    child.LayerDepth = LayerDepth + DrawLayer.Increment;
                Children.Add(child);
            }
            
            _childrenToAdd.Clear();
            _childrenToRemove.Clear();
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsEnabled)
            {
                OnDraw(spriteBatch, gameTime);
                foreach (Control child in Children)
                {
                    if (child.IsEnabled)
                        child.Draw(spriteBatch, gameTime);
                }
            }
        }
        public void AddChild(Control control)
        {
            control.Parent = this;
            if (control.LayerDepth <= LayerDepth)
                control.LayerDepth = LayerDepth + DrawLayer.Increment;
            _childrenToAdd.Add(control);
        }
        public void RemoveChild(Control control)
        {
            _childrenToRemove.Add(control);
            _uiManager.AddChild(control);
        }

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(SpriteBatch spriteBatch, GameTime gameTime) { }
        protected virtual void OnMove() { }
        protected virtual void OnResize() { }
        protected virtual void OnHover() { }
        protected virtual void OnUnhover() { }
        protected virtual void OnPress() { }
        protected virtual void OnRelease() { }

        private void CalculateLayout()
        {
            float xPixels = Layout.X;
            if (Layout.XUnit == LengthUnit.Percent) xPixels = Math.Clamp(Layout.X, 0f, 1f) * Parent.X;
            float yPixels = Layout.Y;
            if (Layout.YUnit == LengthUnit.Percent) yPixels = Math.Clamp(Layout.Y, 0f, 1f) * Parent.Y;
            Width = Layout.Width;
            if (Layout.WidthUnit == LengthUnit.Percent) Width = Math.Clamp(Layout.Width, 0f, 1f) * Parent.Width;
            Height = Layout.Height;
            if (Layout.HeightUnit == LengthUnit.Percent) Height = Math.Clamp(Layout.Height, 0f, 1f) * Parent.Height; 

            switch (Layout.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    X = xPixels;
                    break;
                case HorizontalAlignment.Right:
                    X = Parent.Width - Width - xPixels;
                    break;
                case HorizontalAlignment.Center:
                    X = (Parent.Width + Width) / 2 + xPixels;
                    break;
            }

            switch (Layout.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    Y = yPixels;
                    break;
                case VerticalAlignment.Bottom:
                    Y = Parent.Height - Height - yPixels;
                    break;
                case VerticalAlignment.Center:
                    Y = (Parent.Height + Height) / 2 + yPixels;
                    break;
            }
        }
    }
}

