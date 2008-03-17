using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GammaDraconis.Screens.Menus
{
    class MainMenu : MenuScreen
    {
        public MainMenu(GammaDraconis game)
            : base(game)
        {
        }

        private class Commands
        {
            public static string Play = "Play";
            public static string Quit = "Quit";
        }

        /// <summary>
        /// Sets up the menu items for the main menu
        /// </summary>
        protected override void SetupMenuItems()
        {
			float itemY = Game.Window.ClientBounds.Height / 2.2f;
            float itemX = 75.0f;
            menuItems = new MenuItem[2];
            menuItems[0] = new MenuItem("Play", this, new Vector2(itemX, itemY), Commands.Play);
            menuItems[1] = new MenuItem("Quit", this, new Vector2(itemX, itemY), Commands.Quit);
        }

        /// <summary>
        /// The background image for the screen
        /// </summary>
        /// <returns>The name of the background image</returns>
        protected override string GetBackgroundImage()
        {
            return "Resources/Textures/MenuBackgrounds/MainMenu";
        }

        /// <summary>
        /// Activates a menu item
        /// </summary>
        /// <param name="item">The menu item selected</param>
        protected override void ItemSelected(String command)
        {
            if (command.Equals(Commands.Play))
            {
                // TODO: Play the game
                Console.WriteLine("There isn't a game to play yet! Start making it!");
            }
            else if (command.Equals(Commands.Quit))
            {
                Game.Exit();
            }
        }

        protected override void Cancel()
        {
            Console.WriteLine("There isn't a game to play yet! Start making it!");
        }

    }
}
