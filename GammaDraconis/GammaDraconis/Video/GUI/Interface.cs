using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GammaDraconis.Video.GUI
{
    /// <summary>
    /// An Interface represents a (possibly) interactive two-dimensional
    /// display, such as a menu or HUD.
    /// </summary>
    class Interface : InterfaceComponent
    {
        private List<InterfaceComponent> subComponents;
        public void AddComponent(InterfaceComponent component)
        {
            subComponents.Add(component);
            component.Visible = Visible;
            component.Enabled = Enabled;
        }
        public void RemoveComponent(InterfaceComponent component)
        {
            subComponents.Remove(component);
        }

        public Interface(GammaDraconis game)
            : base(game)
        {
            subComponents = new List<InterfaceComponent>();
        }

        protected override void OnVisibleChanged(object sender, EventArgs args)
        {
            if (subComponents != null)
            {
                foreach (InterfaceComponent component in subComponents)
                {
                    component.Visible = Visible;
                }
            }
            base.OnVisibleChanged(sender, args);
        }

        protected override void OnEnabledChanged(object sender, EventArgs args)
        {
            if (subComponents != null)
            {
                foreach (InterfaceComponent component in subComponents)
                {
                    component.Enabled = Enabled;
                }
            }
            base.OnEnabledChanged(sender, args);
        }

        internal override void Draw(GameTime gameTime, Vector2 position, Vector2 scale, float rotation)
        {
            foreach (InterfaceComponent component in subComponents)
            {
                component.Draw(gameTime, position + RelativePosition, scale * RelativeScale, rotation + RelativeRotation);
            }
        }
    }
}
