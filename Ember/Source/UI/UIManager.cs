using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.UI
{
    public sealed class UIManager : Element
    {
        public UIManager(Viewport viewport)
        {
            Width = viewport.Width;
            Height = viewport.Height;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Element element in CollectElements(Children))
            {
                UpdateElementState(element);
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            base.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }

        private IEnumerable<Element> CollectElements(List<Element> elements)
        {
            foreach (Element element in elements)
            {
                yield return element;

                foreach (Element child in CollectElements(element.Children))
                    yield return child;
            }
        }
        private void UpdateElementState(Element element) 
        {
            if (element.AbsoluteBounds.Contains(Input.MousePosition))
            {
                if (!element.IsHovered)
                {
                    element.IsHovered = true;
                    element.Hovered?.Invoke();
                }
                if (Input.MouseDown(MouseButton.Left) && !element.IsHeld)
                {
                    element.IsHeld = true;
                    element.Pressed?.Invoke();
                }
            }
            else if (element.IsHovered)
            {
                element.IsHovered = false;
                element.Unhovered?.Invoke();
            }
            if (Input.MouseUp(MouseButton.Left) && element.IsHeld)
            {
                element.IsHeld = false;
                element.Released?.Invoke();
            }
        }
    }
}
