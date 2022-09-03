using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ember.Graphics;

namespace Ember.UI
{
    public enum ButtonState
    {
        None,
        Hovered,
        Held
    }
    public enum HorizontalAlignment
    {
        None,
        Left,
        Right,
        Center
    }
    public enum VerticalAlignment
    {
        None,
        Top,
        Bottom,
        Center
    }

    public class Element
    {
        public Element Parent;
        public List<Element> Children = new List<Element>();
        public Vector2 Origin;
        public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.None;
        public VerticalAlignment VerticalAlignment = VerticalAlignment.None;
        public bool IsHovered;
        public bool IsHeld;

        private float _x;
        private float _y;
        private float _width;
        private float _height;
        private float _layerDepth = DrawLayer.UI;

        public Element()
        {
            Moved += OnMove;
            Resized += OnResize;
            Hovered += OnHover;
            Unhovered += OnUnhover;
            Pressed += OnPress;
            Released += OnRelease;
        }

        public virtual float X
        {
            get
            {
                switch(HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        return _x;
                    case HorizontalAlignment.Right:
                        return Parent.Bounds.Right - Width + _x;
                    case HorizontalAlignment.Center:
                        return (Parent.Bounds.Right - Width) / 2 + _x;
                    default:
                        return _x;
                }
            }
            set
            {
                _x = value;
                Moved?.Invoke();
            }
        }
        public virtual float Y
        {
            get
            {
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        return _y;
                    case VerticalAlignment.Bottom:
                        return Parent.Bounds.Bottom - Height + _y;
                    case VerticalAlignment.Center:
                        return (Parent.Bounds.Bottom - Height) / 2 + _y;
                    default:
                        return _y;
                }
            }
            set
            {
                _y = value;
                Moved?.Invoke();
            }
        }
        public virtual float Width
        {
            get => _width;
            set
            {
                _width = value;
                Resized?.Invoke();
            }
        }
        public virtual float Height
        {
            get => _height;
            set
            {
                _height = value;
                Resized?.Invoke();
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

        public float LayerDepth
        {
            get => _layerDepth;
            set
            {
                _layerDepth = value;
                foreach (Element element in Children)
                    element.LayerDepth = _layerDepth + DrawLayer.Increment;
            }
        }

        public bool IsEnabled { get; set; } = true;

        public Action Moved;
        public Action Resized;
        public Action Hovered;
        public Action Unhovered;
        public Action Pressed;
        public Action Released;

        protected virtual void OnUpdate(GameTime gameTime) { }
        protected virtual void OnDraw(SpriteBatch spriteBatch, GameTime gameTime) { }
        protected virtual void OnMove() { }
        protected virtual void OnResize() { }
        protected virtual void OnHover() { }
        protected virtual void OnUnhover() { }
        protected virtual void OnPress() { }
        protected virtual void OnRelease() { }

        public virtual void Update(GameTime gameTime)
        {
            if (IsEnabled)
            {
                OnUpdate(gameTime);
                foreach (Element child in Children)
                {
                    if (child.IsEnabled)
                        child.Update(gameTime);
                }
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsEnabled)
            {
                OnDraw(spriteBatch, gameTime);
                foreach (Element child in Children)
                {
                    if (child.IsEnabled)
                        child.Draw(spriteBatch, gameTime);
                }
            }
        }

        public void AddChild(Element element)
        {
            element.Parent = this;
            if (element.LayerDepth < LayerDepth)
                element.LayerDepth = LayerDepth + DrawLayer.Increment;
            Children.Add(element);
        }
        public void RemoveChild(Element element)
        {
            element.Parent = null;
            Children.Remove(element);
        }
    }
}

