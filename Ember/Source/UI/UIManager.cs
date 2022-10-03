using System.Collections.Generic;
using Ember.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.UI
{
    public sealed class UIManager : Control
    {
        public readonly GraphicsContext GraphicsContext;
        
        public UIManager(Viewport viewport, GraphicsContext graphicsContext)
        {
            Width.Value = viewport.Width;
            Height.Value = viewport.Height;
            GraphicsContext = graphicsContext;
            
            UIManager = this;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Control control in CollectControls(Children))
            {
                UpdateControlState(control);
            }
            base.Update(gameTime);
        }
        public override void Draw(GraphicsContext graphicsContext, GameTime gameTime, Vector2 parentPosition)
        {
            graphicsContext.SpriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            base.Draw(graphicsContext, gameTime, parentPosition);
            graphicsContext.SpriteBatch.End();
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
