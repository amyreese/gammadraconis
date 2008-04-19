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
    public class Interface : InterfaceComponent
    {
        private List<InterfaceComponent> subComponents;

        public void AddComponent(InterfaceComponent component)
        {
            subComponents.Add(component);
            component.Enabled = Enabled;
        }

        public void AddComponents(InterfaceComponent[] components)
        {
            foreach (InterfaceComponent component in components)
            {
                subComponents.Add(component);
                component.Enabled = Enabled;
            }
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
            CalculateResultingValues(position, scale, rotation, out position, out scale, out rotation);
            foreach (InterfaceComponent component in subComponents)
            {
                if(component.Visible)
                    component.Draw(gameTime, position, scale, rotation);
            }
        }
    }
}
