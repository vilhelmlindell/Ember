using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.GUI
{
    public enum ButtonState
    {
        None,
        Hovered,
        Held
    };

    public class Element
    {
        public Element Parent;
        public List<Element> Children = new List<Element>();
        public bool IsHovered;
        public bool IsHeld;
        public float LayerDepth;
        public Vector2 Origin;

        private float _x;
        private float _y;
        private float _width = 100f;
        private float _height = 100f;

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
            get => _x;
            set
            {
                _x = value;
                Moved?.Invoke();
            }
        }
        public virtual float Y
        {
            get => _y;
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

        public virtual float PrefWidth { get; set; }
        public virtual float PrefHeight { get; set; }

        public virtual Vector2 Position
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public virtual Vector2 AbsolutePosition
        {
            get
            {
                if (Parent != null)
                    return Parent.AbsolutePosition;
                return Position;
            }
        }
        public virtual Vector2 Size
        {
            get => new Vector2(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }
        public virtual RectangleF Bounds
        {
            get => new RectangleF(Position, Size);
            set
            {
                Position = new Vector2(value.X, value.Y);
                Size = new Vector2(value.Width, value.Height);
            }
        }
        public virtual RectangleF AbsoluteBounds
        {
            get
            {
                return new RectangleF(AbsolutePosition, Size);
            }
        }

        public virtual bool IsEnabled { get; set; } = true;

        public Action Moved;
        public Action Resized;
        public Action Hovered;
        public Action Unhovered;
        public Action Pressed;
        public Action Released;

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
                foreach (Element child in Children)
                {
                    if (child.IsEnabled)
                        child.Draw(spriteBatch, gameTime);
                }
            }
        }
    }
}
