using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using GammaDraconis.Core.Input;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Screens.Menus
{
    abstract class MenuScreen : Screen
    {
        // The items displayed in the menu
        protected MenuItem[] menuItems;

        // The index of the currently selected menu item
        protected int menuItemIndex = 0;

        /// <summary>
        /// Constructor for the screen
        /// </summary>
        /// <param name="game">Gamma Draconis instance</param>
        public MenuScreen(GammaDraconis game)
            : base(game)
        {
            Sprite backgroundImage = new Sprite(game);
            backgroundImage.textureName = GetBackgroundImageName();
            // TODO: Remove scale when we have better background images
            backgroundImage.RelativeScale = new Vector2(1024.0f / 800.0f, 768.0f / 600.0f);
            screenInterface.AddComponent(backgroundImage);
            SetupMenuItems();
            for (int index = 0; index < menuItems.Length; index++)
            {
                bool selected = index == menuItemIndex;
                menuItems[index].RelativeScale = selected ? GetSelectedScale() : GetUnselectedScale();
                menuItems[index].color = selected ? GetSelectedColor() : GetUnselectedColor();
                menuItems[index].spriteFontName = GetFontName();
            }
            ready = true;
        }

        /// <summary>
        /// Prepare the menu items to be displayed on this menu
        /// </summary>
        protected abstract void SetupMenuItems();

        protected void AutoPositionMenuItems()
        {
            for (int index = 0; index < menuItems.Length; index++)
            {
                menuItems[index].RelativePosition = GetMenuItemSpacing() * index;
            }
        }

        #region Graphics Stuff
        /// <summary>
        /// The background image to be used for this menu
        /// </summary>
        /// <returns>The background image to be used for this menu</returns>
        protected abstract String GetBackgroundImageName();

        /// <summary>
        /// The name of the font used for this menu
        /// </summary>
        /// <returns>The name of the font used for this menu</returns>
        internal virtual String GetFontName()
        {
            return "Resources/Fonts/Menu";
        }

        /// <summary>
        /// The color used for menu items that are not selected
        /// </summary>
        /// <returns>The color used for menu items that are not selected</returns>
        internal virtual Color GetUnselectedColor()
        {
            return new Color(255, 50, 45);
        }

        /// <summary>
        /// The color used for menu items that are selected
        /// </summary>
        /// <returns>The color used for menu items that are selected</returns>
        internal virtual Color GetSelectedColor()
        {
            return new Color(255, 255, 255);
        }

        /// <summary>
        /// The scale used for menu items that are not selected
        /// </summary>
        /// <returns>The scale used for menu items that are not selected</returns>
        internal virtual Vector2 GetUnselectedScale()
        {
            return Vector2.One;
        }

        /// <summary>
        /// The scale used for menu items that are selected
        /// </summary>
        /// <returns>The scale used for menu items that are selected</returns>
        internal virtual Vector2 GetSelectedScale()
        {
            return new Vector2(1.5f, 1.5f);
        }

        /// <summary>
        /// The spacing used for menu items
        /// </summary>
        /// <returns>The spacing used for menu items</returns>
        internal virtual Vector2 GetMenuItemSpacing()
        {
            return new Vector2(0.0f, 64.0f);
        }
        #endregion

        protected abstract void ItemSelected(String command);

        protected abstract void Cancel();


        /// <summary>
        /// Check to see if the menu is being cycled through
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            #region Input Commands

            #region Menu Item Selection commands
            int oldMenuItemIndex = menuItemIndex;
            menuItems[menuItemIndex].color = GetUnselectedColor();
            menuItems[menuItemIndex].RelativeScale = GetUnselectedScale();
            if (input.inputPressed(MenuInput.Commands.Up))
            {
                do
                {
                    menuItemIndex--;
                    if (menuItemIndex < 0)
                    {
                        menuItemIndex = menuItems.Length - 1;
                    }
                } while (!menuItems[menuItemIndex].Visible && menuItemIndex != oldMenuItemIndex);
                if (menuItemIndex == oldMenuItemIndex)
                {
                    throw new Exception("Bad menu!");
                }
            }

            if (input.inputPressed(MenuInput.Commands.Down))
            {
                do
                {
                    menuItemIndex++;
                    if (menuItemIndex >= menuItems.Length)
                    {
                        menuItemIndex = 0;
                    }
                } while (!menuItems[menuItemIndex].Visible && menuItemIndex != oldMenuItemIndex);
                if (menuItemIndex == oldMenuItemIndex)
                {
                    throw new Exception("Bad menu!");
                }
            }
            menuItems[menuItemIndex].color = GetSelectedColor();
            menuItems[menuItemIndex].RelativeScale = GetSelectedScale();
            #endregion

            if (input.inputPressed(MenuInput.Commands.Select))
            {
                ItemSelected(menuItems[menuItemIndex].command);
            }

            if (input.inputPressed(MenuInput.Commands.Cancel))
            {
                Cancel();
            }
            #endregion

            base.Update(gameTime);
        }
    }
}
