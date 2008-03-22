using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Screens.Menus
{
    class MainMenu : MenuScreen
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public MainMenu(GammaDraconis game)
            : base(game)
        {
        }

        /// <summary>
        /// The commands that can be used for this menu
        /// </summary>
        private class Commands
        {
            public static string Play = "Play";
            public static string Controls = "Controls";
            public static string ToggleFullscreen = "ToggleFullscreen";
            public static string Quit = "Quit";
        }

        /// <summary>
        /// Sets up the menu items for the main menu
        /// </summary>
        protected override void SetupMenuItems()
        {
            Interface menuRegion = new Interface(gammaDraconis);
            menuRegion.RelativePosition = new Vector2(100.0f, Game.Window.ClientBounds.Height / 1.8f);
            screenInterface.AddComponent(menuRegion);

            menuItems = new MenuItem[4];
            menuItems[0] = new MenuItem(gammaDraconis, Commands.Play);
            menuItems[0].text = "Play";
            menuItems[1] = new MenuItem(gammaDraconis, Commands.Controls);
            menuItems[1].text = "Controls";
            menuItems[2] = new MenuItem(gammaDraconis, Commands.ToggleFullscreen);
            menuItems[2].text = "Toggle Fullscreen";
            menuItems[3] = new MenuItem(gammaDraconis, Commands.Quit);
            menuItems[3].text = "Quit";

            AutoPositionMenuItems();
            
            foreach (MenuItem item in menuItems)
            {
                menuRegion.AddComponent(item);
            }
        }

        /// <summary>
        /// The background image for the screen
        /// </summary>
        /// <returns>The name of the background image</returns>
        protected override string GetBackgroundImageName()
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
                gammaDraconis.changeState(GammaDraconis.GameStates.GameLoading);
            }
            else if (command.Equals(Commands.Quit))
            {
                Game.Exit();
            }
            else if (command.Equals(Commands.ToggleFullscreen))
            {
                gammaDraconis.ToggleFullscreen();
            }
            else
            {
                Console.WriteLine("Not yet implemented!");
            }
        }

        protected override void Cancel()
        {
            ItemSelected(Commands.Play);
        }
    }
}
