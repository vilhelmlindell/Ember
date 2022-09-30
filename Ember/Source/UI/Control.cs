using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ember.Graphics;

namespace Ember.UI
{
    public enum Alignment
    {
        TopLeft,
        Top,
        TopRight,
        BottomLeft,
        Bottom,
        BottomRight,
        Left,
        Right,
        Center
    }
    public enum LengthUnit
    {
        Pixels,
        Percent
    }


    public class Length
    {

        private float _value;
        private LengthUnit _unit;

        public Length()
        {
            _value = 0f;
            _unit = LengthUnit.Pixels;
        }
        public Length(float value = 0f, LengthUnit lengthUnit = LengthUnit.Pixels)
        {
            Value = value;
            Unit = lengthUnit;
        }
        
        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                WasChanged?.Invoke();
            }
        }
        public LengthUnit Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                WasChanged?.Invoke();
            }
        }

        public Action WasChanged;

    }

    /// <summary>
    /// Describes the dimensions of a control
    /// </summary>
    public class Layout
    {
        protected bool ShouldCalculateLayout = false;
        
        public Layout()
        {
            X = new Length();
            Y = new Length();
            Width = new Length();
            Height = new Length();
            OriginX = new Length();
            OriginY = new Length();

            X.WasChanged += () => ShouldCalculateLayout = true;
            Y.WasChanged += () => ShouldCalculateLayout = true;
            Width.WasChanged += () => ShouldCalculateLayout = true;
            Height.WasChanged += () => ShouldCalculateLayout = true;
            OriginX.WasChanged += () => ShouldCalculateLayout = true;
            OriginY.WasChanged += () => ShouldCalculateLayout = true;
        }

        public Length X { get; set; }
        public Length Y { get; set; }
        public Length Width { get; set; }
        public Length Height { get; set; }
        public Length OriginX { get; set; }
        public Length OriginY { get; set; }

        public void SetPosition(Alignment alignment)
        {
            X.Unit = LengthUnit.Percent;
            Y.Unit = LengthUnit.Percent;
            switch (alignment)
            {
                case Alignment.TopLeft:
                    X.Value = 0f;
                    Y.Value = 0f;
                    break;
                case Alignment.Top:
                    X.Value = 0.5f;
                    Y.Value = 0f;
                    break;
                case Alignment.TopRight:
                    X.Value = 1f;
                    Y.Value = 0f;
                    break;
                case Alignment.BottomLeft:
                    X.Value = 0f;
                    Y.Value = 1f;
                    break;
                case Alignment.Bottom:
                    X.Value = 0.5f;
                    Y.Value = 1f;
                    break;
                case Alignment.BottomRight:
                    X.Value = 1f;
                    Y.Value = 1f;
                    break;
                case Alignment.Left:
                    X.Value = 0f;
                    Y.Value = 0.5f;
                    break;
                case Alignment.Right:
                    X.Value = 1f;
                    Y.Value = 0.5f;
                    break;
                case Alignment.Center:
                    X.Value = 0.5f;
                    Y.Value = 0.5f;
                    break;
            }
        }
        public void SetOrigin(Alignment alignment)
        {
            OriginX.Unit = LengthUnit.Percent;
            OriginY.Unit = LengthUnit.Percent;
            switch (alignment)
            {
                case Alignment.TopLeft:
                    OriginX.Value = 0f;
                    OriginY.Value = 0f;
                    break;
                case Alignment.Top:
                    OriginX.Value = 0.5f;
                    OriginY.Value = 0f;
                    break;
                case Alignment.TopRight:
                    OriginX.Value = 1f;
                    OriginY.Value = 0f;
                    break;
                case Alignment.BottomLeft:
                    OriginX.Value = 0f;
                    OriginY.Value = 1f;
                    break;
                case Alignment.Bottom:
                    OriginX.Value = 0.5f;
                    OriginY.Value = 1f;
                    break;
                case Alignment.BottomRight:
                    OriginX.Value = 1f;
                    OriginY.Value = 1f;
                    break;
                case Alignment.Left:
                    OriginX.Value = 0f;
                    OriginY.Value = 0.5f;
                    break;
                case Alignment.Right:
                    OriginX.Value = 1f;
                    OriginY.Value = 0.5f;
                    break;
                case Alignment.Center:
                    OriginX.Value = 0.5f;
                    OriginY.Value = 0.5f;
                    break;
            }
        }

        protected virtual void CalculateLayout() {}
    }

    /// <summary>
    /// Base ui element
    /// </summary>
    public class Control : Layout
    {
        public List<Control> Children = new List<Control>();
        public bool IsHovered;
        public bool IsHeld;
        
        internal UIManager UIManager;
        
        private readonly List<Control> _childrenToAdd = new List<Control>();
        private readonly List<Control> _childrenToRemove = new List<Control>();
        private float _x;
        private float _y;
        private float _width;
        private float _height;
        private float _layerDepth = DrawLayer.UI;
        
        
        public Control()
        {
            Moved += OnMove;
            Resized += OnResize;
            Hovered += OnHover;
            Unhovered += OnUnhover;
            Pressed += OnPress;
            Released += OnRelease;
        }

        public float ActualX
        {
            get => _x;
            private set
            {
                _x = value;
                Moved?.Invoke();
            }
        }
        public float ActualY
        {
            get => _y;
            private set
            {
                _y = value;
                Moved?.Invoke();
            }
        }
        public float ActualWidth
        {
            get => _width;
            private set
            {
                _width = value;
                Resized?.Invoke();
            }
        }
        public float ActualHeight
        {
            get => _height;
            private set
            {
                _height = value;
                Resized?.Invoke();
            }
        }
        public Vector2 Position => new (ActualX, ActualY);
        public Vector2 AbsolutePosition
        {
            get
            {
                if (Parent != null)
                    return Parent.AbsolutePosition + Position;
                return Position;
            }
        }
        private Vector2 Size => new(ActualWidth, ActualHeight);
        public RectangleF Bounds => new RectangleF(Position, Size);
        public RectangleF AbsoluteBounds => new RectangleF(AbsolutePosition, Size);
        
        public Control Parent { get; private set; }
        public bool IsEnabled { get; set; } = true;
        
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

        public Action Moved;
        public Action Resized;
        public Action Hovered;
        public Action Unhovered;
        public Action Pressed;
        public Action Released;
        
        public virtual void Update(GameTime gameTime)
        {
            if (IsEnabled)
            {
                foreach (Control child in Children)
                {
                    if (child.IsEnabled)
                        child.Update(gameTime);
                }

                Children = Children.Except(_childrenToRemove).ToList();

                foreach (Control child in _childrenToAdd)
                {
                    if (child.LayerDepth <= LayerDepth)
                        child.LayerDepth = LayerDepth + DrawLayer.Increment;
                    Children.Add(child);
                }

                _childrenToAdd.Clear();
                _childrenToRemove.Clear();

                if (ShouldCalculateLayout)
                {
                    CalculateLayout();
                    ShouldCalculateLayout = false;
                }
            }
        }
        public virtual void Draw(GraphicsContext graphicsContext, GameTime gameTime)
        {
            if (IsEnabled)
            {
                foreach (Control child in Children)
                {
                    if (child.IsEnabled)
                        child.Draw(graphicsContext, gameTime);
                }
            }
        }
        public void AddChild(Control control)
        {
            control.UIManager = UIManager;
            control.Parent = this;
            if (control.LayerDepth <= LayerDepth)
                control.LayerDepth = LayerDepth + DrawLayer.Increment;
            _childrenToAdd.Add(control);
            OnParentAdded();
        }
        public void RemoveChild(Control control)
        {
            _childrenToRemove.Add(control);
            OnParentRemoved();
        }

        protected virtual void OnMove() {}
        protected virtual void OnResize() {}
        protected virtual void OnHover() {}
        protected virtual void OnUnhover() {}
        protected virtual void OnPress() {}
        protected virtual void OnRelease() {}
        protected virtual void OnParentAdded() {}
        protected virtual void OnParentRemoved() {}

        protected override void CalculateLayout()
        {
            ActualX = X.Value;
            if (X.Unit == LengthUnit.Percent)
            {
                ActualX = Math.Clamp(X.Value, 0f, 1f) * Parent.ActualWidth;
            }

            ActualY = Y.Value;
            if (Y.Unit == LengthUnit.Percent) 
                ActualY = Math.Clamp(Y.Value, 0f, 1f) * Parent.ActualHeight;
            
            ActualWidth = Width.Value;
            if (Width.Unit == LengthUnit.Percent) 
                ActualWidth = Math.Clamp(Width.Value, 0f, 1f) * Parent.ActualWidth;
            
            ActualHeight = Height.Value;
            if (Height.Unit == LengthUnit.Percent) 
                ActualHeight = Math.Clamp(Height.Value, 0f, 1f) * Parent.ActualHeight;

            if (OriginX.Unit == LengthUnit.Pixels)
                ActualX -= OriginX.Value;
            else
                ActualX -= ActualWidth * OriginX.Value;
            
            if (OriginY.Unit == LengthUnit.Pixels)
                ActualY -= OriginY.Value;
            else
                ActualY -= ActualHeight * OriginY.Value;

            //switch (HorizontalAlignment)
            //{
            //    case HorizontalAlignment.Left:
            //        ActualX = xPixels;
            //        break;
            //    case HorizontalAlignment.Right:
            //        ActualX = Parent.ActualWidth - ActualWidth - xPixels;
            //        break;
            //    case HorizontalAlignment.Center:
            //        ActualX = (Parent.ActualWidth - ActualWidth) / 2 + xPixels;
            //        break;
            //}

            //switch (VerticalAlignment)
            //{
            //    case VerticalAlignment.Top:
            //        ActualY = yPixels;
            //        break;
            //    case VerticalAlignment.Bottom:
            //        ActualY = Parent.ActualHeight - ActualHeight - yPixels;
            //        break;
            //    case VerticalAlignment.Center:
            //        ActualY = (Parent.ActualHeight - ActualHeight) / 2 + yPixels;
            //        break;
            //}
        }
    }
}

