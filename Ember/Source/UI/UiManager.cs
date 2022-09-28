using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.UI
{
    public sealed class UiManager : Control
    {
        public UiManager(Viewport viewport)
        {
            Width = viewport.Width;
            Height = viewport.Height;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (Control control in CollectControls(Children))
            {
                UpdateControlState(control);
            }
            base.Update(gameTime);
        }
        protected override void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            base.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }

        private IEnumerable<Control> CollectControls(List<Control> controls)
        {
            foreach (Control control in controls)
            {
                yield return control;

                foreach (Control child in CollectControls(control.Children))
                    yield return child;
            }
        }
        private static void UpdateControlState(Control control) 
        {
            if (control.AbsoluteBounds.Contains(Input.MousePosition))
            {
                if (!control.IsHovered)
                {
                    control.IsHovered = true;
                    control.Hovered?.Invoke();
                }
                if (Input.MouseDown(MouseButton.Left) && !control.IsHeld)
                {
                    control.IsHeld = true;
                    control.Pressed?.Invoke();
                }
            }
            else if (control.IsHovered)
            {
                control.IsHovered = false;
                control.Unhovered?.Invoke();
            }
            if (Input.MouseUp(MouseButton.Left) && control.IsHeld)
            {
                control.IsHeld = false;
                control.Released?.Invoke();
            }
        }
    }
}
