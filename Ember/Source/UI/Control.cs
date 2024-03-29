using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ember.Graphics;

namespace Ember.UI
{
    public enum Overflow
    {
        Visible,
        Hidden,
        Scroll,
    }

    /// <summary>
    /// Base ui element
    /// </summary>
    public class Control : Layout
    {
        public List<Control> Children = new List<Control>();
        public Overflow Overflow = Overflow.Visible;
        public bool IsHovered;
        public bool IsHeld;
        
        internal UIManager UIManager;
        
        private readonly List<Control> _childrenToAdd = new List<Control>();
        private readonly List<Control> _childrenToRemove = new List<Control>();
        private RenderTarget2D _renderTarget;
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

        public float OverflowWidth
        {
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
        public Vector2 Size => new(ActualWidth, ActualHeight);
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
        public virtual void Draw(GraphicsContext graphicsContext, GameTime gameTime, Vector2 parentPosition)
        {
            if (IsEnabled && UIManager != null)
            {
                graphicsContext.GraphicsDevice.SetRenderTarget(_renderTarget);
                foreach (Control child in Children)
                {
                    if (child.IsEnabled)
                        child.Draw(graphicsContext, gameTime, parentPosition);
                }
            }
        }

        public void UpdateRenderTargetSize()
        {
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

        protected virtual void OnParentAdded()
        {
            _renderTarget = new RenderTarget2D(UIManager.GraphicsContext.GraphicsDevice,
                Convert.ToInt32(ActualWidth), Convert.ToInt32(ActualHeight));
        }
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
        }
    }
}


