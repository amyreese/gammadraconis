using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Video.Interface;

namespace GammaDraconis.Screens.Menus
{
    class MenuItem : TextComponent
    {
        public readonly String command;

        /// <summary>
        /// Creates the menu item
        /// </summary>
        /// <param name="text">The text of the menu item</param>
        /// <param name="screen">The screen this menu item is on</param>
        /// <param name="position">The location of this menu item</param>
        public MenuItem(GammaDraconis game, String command) : base(game)
        {
            this.command = command;
        }
    }
}
