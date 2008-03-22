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
            public static string Quit = "Quit";
        }

        /// <summary>
        /// Sets up the menu items for the main menu
        /// </summary>
        protected override void SetupMenuItems()
        {
            Interface menuRegion = new Interface(gammaDraconis);
            menuRegion.RelativePosition = new Vector2(100.0f, Game.Window.ClientBounds.Height / 1.8f);
            menuItems = new MenuItem[3];
            menuItems[0] = new MenuItem(gammaDraconis, Commands.Play);
            menuItems[0].text = "Play";
            menuRegion.AddComponent(menuItems[0]);
            menuItems[1] = new MenuItem(gammaDraconis, Commands.Controls);
            menuItems[1].text = "Controls";
            menuRegion.AddComponent(menuItems[1]);
            menuItems[2] = new MenuItem(gammaDraconis, Commands.Quit);
            menuItems[2].text = "Quit";
            menuRegion.AddComponent(menuItems[2]);
            screenInterface.AddComponent(menuRegion);
            AutoPositionMenuItems();
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
            else
            {
                Console.WriteLine("Not yet implemented!");
            }
        }

        protected override void Cancel()
        {
            Console.WriteLine("There isn't a game to play yet! Start making it!");
        }

    }
}
