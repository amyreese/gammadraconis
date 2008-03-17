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
            SetupMenuItems();
            for (int index = 0; index < menuItems.Length; index++)
            {
                menuItems[index].Selected = index == menuItemIndex;
            }
        }

        protected static MenuInput input = new MenuInput();

        #region Graphics Stuff
        // The background image
        private Texture2D screenImage;

        // The sprite batch for the screen
        protected SpriteBatch _spriteBatch;
        public SpriteBatch spriteBatch
        {
            get
            {
                return _spriteBatch;
            }
            protected set
            {
                _spriteBatch = value;
            }
        }

        // The font for the screen
        protected SpriteFont _spriteFont;
        public SpriteFont spriteFont
        {
            get
            {
                return _spriteFont;
            }
            protected set
            {
                _spriteFont = value;
            }
        }

        /// <summary>
        /// Load the background image, font, and menu items for the screen
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Game.Content.Load<SpriteFont>("Resources/Fonts/Menu.spritefont");
			screenImage = Game.Content.Load<Texture2D>(GetBackgroundImage()); 
            base.LoadContent();
        }

        /// <summary>
        /// Prepare the menu items to be displayed on this menu
        /// </summary>
        protected abstract void SetupMenuItems();
        
        /// <summary>
        /// The background image to be used for this menu
        /// </summary>
        /// <returns>The background image to be used for this menu</returns>
        protected abstract String GetBackgroundImage();

        /// <summary>
        /// Draw the background image and the menu items
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer,
                Color.Black, 1.0f, 0);
			
			spriteBatch.Begin();
			spriteBatch.Draw(screenImage, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
			spriteBatch.End();
			
			if (menuItems != null)
            {
                for (int menuItemIndex = 0; menuItemIndex < menuItems.Length; menuItemIndex++)
                {
                    menuItems[menuItemIndex].Draw(gameTime);
                }
            }
            base.Draw(gameTime);
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

            input.update();

            if (input.inputPressed(MenuInput.Commands.Up))
            {
                menuItems[menuItemIndex].Selected = false;
				do
				{
					menuItemIndex--;
					if (menuItemIndex < 0)
					{
						menuItemIndex = menuItems.Length - 1;
					}
				} while (!menuItems[menuItemIndex].Visible);
                menuItems[menuItemIndex].Selected = true;
            }

            if (input.inputPressed(MenuInput.Commands.Down))
            {
                menuItems[menuItemIndex].Selected = false;
				do
				{
					menuItemIndex++;
					if (menuItemIndex >= menuItems.Length)
					{
						menuItemIndex = 0;
					}
				} while (!menuItems[menuItemIndex].Visible);
				menuItems[menuItemIndex].Selected = true;
            }

            if( input.inputPressed(MenuInput.Commands.Select) )
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
