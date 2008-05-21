using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Types;
using GammaDraconis.Video;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Screens.Menus
{
    class ControlsMenu : MenuScreen
    {
        private GameObject skybox;
        private Selector pageSelector;
        private Sprite xbox360PadFront, xbox360PadTop;
        private Text keyboardWASD, keyboardNumPad;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="game">The game instance.</param>
        public ControlsMenu(GammaDraconis game)
            : base(game)
        {
            skybox = new Skybox();
            screenScene.track(skybox, GO_TYPE.SKYBOX);

            xbox360PadFront = new Sprite(game);
            xbox360PadFront.textureName = "Resources/Textures/Xbox 360 Controller Front";
            xbox360PadFront.RelativePosition = new Vector2(112.0f, 100.0f);
            screenInterface.AddComponent(xbox360PadFront);

            xbox360PadTop = new Sprite(game);
            xbox360PadTop.textureName = "Resources/Textures/Xbox 360 Controller Top";
            xbox360PadTop.RelativePosition = new Vector2(112.0f, 480.0f);
            screenInterface.AddComponent(xbox360PadTop);

            keyboardWASD = new Text(game, "W - Accelerate Forward\nS - Accelerate Backward\nA - Roll Left\nD - Roll Right\n" +
                "Arrow Keys - Steer Ship/Navigate Menus\n\nSpace - Fire Beam Weapons\nEnter - Fire Mine Weapons\nQ - Reset Ship\n\n" +
                "P - Pause\nEnter - Start Game\nEsc - Back/Quit Game");
            keyboardWASD.RelativePosition = new Vector2(1024 / 2, 768 / 2);
            keyboardWASD.center = true;
            keyboardWASD.color = Color.White;
            keyboardWASD.SpriteFontName = "Resources/Fonts/Menu";
            screenInterface.AddComponent(keyboardWASD);

            keyboardNumPad = new Text(game, "0 - Accelerate Forward\n5 - Accelerate Backward\n7 - Roll Left\n9 - Roll Right\n" +
                "Arrow Keys - Steer Ship/Navigate Menus\n\n1 - Fire Beam Weapons\n3 - Fire Mine Weapons\n. - Reset Ship\n\n" +
                "P - Pause\nEnter - Start Game\nEsc - Back/Quit Game");
            keyboardNumPad.RelativePosition = new Vector2(1024 / 2, 768 / 2);
            keyboardNumPad.center = true;
            keyboardNumPad.color = Color.White;
            keyboardNumPad.SpriteFontName = "Resources/Fonts/Menu";
            screenInterface.AddComponent(keyboardNumPad);

            pageSelector = new Selector(game, "Xbox 360 Controller", "Keyboard - WASD", "Keyboard - NumPad");
            pageSelector.RelativePosition = new Vector2(1024 / 2, 40.0f);
            pageSelector.center = true;
            pageSelector.color = Color.White;
            pageSelector.SpriteFontName = "Resources/Fonts/Menu";
            screenInterface.AddComponent(pageSelector);
        }

        /// <summary>
        /// The commands that can be used for this menu
        /// </summary>
        private class Commands
        {
            public static string Back = "Back";
        }


        /// <summary>
        /// Prepare the menu items to be displayed on this menu
        /// </summary>
        protected override void SetupMenuItems()
        {
            menuItems = new MenuItem[1];
            menuItems[0] = new MenuItem(gammaDraconis, Commands.Back);
            menuItems[0].text = "";
        }

        /// <summary>
        /// The background image to be used for this menu
        /// </summary>
        /// <returns>The background image to be used for this menu</returns>
        protected override string GetBackgroundImageName()
        {
            return null;
        }

        /// <summary>
        /// Take action when an item is selected.
        /// </summary>
        /// <param name="command">The selected item.</param>
        protected override void ItemSelected(string command)
        {
            gammaDraconis.changeState(GammaDraconis.GameStates.MainMenu);
        }

        /// <summary>
        /// Cancel out of this menu.
        /// </summary>
        protected override void Cancel()
        {
            ItemSelected(Commands.Back);
        }

        /// <summary>
        /// Check for input and manage state.
        /// </summary>
        /// <param name="gameTime">The current time.</param>
        public override void Update(GameTime gameTime)
        {
            if(input.inputPressed(Core.Input.MenuInput.Commands.Right))
                pageSelector.NextSelection();
            else if(input.inputPressed(Core.Input.MenuInput.Commands.Left))
                pageSelector.PrevSelection();
            else if(input.inputPressed(Core.Input.MenuInput.Commands.Cancel))
                Cancel();

            xbox360PadFront.Visible = (pageSelector.CurrentSelection == "Xbox 360 Controller");
            xbox360PadTop.Visible = (pageSelector.CurrentSelection == "Xbox 360 Controller");
            keyboardWASD.Visible = (pageSelector.CurrentSelection == "Keyboard - WASD");
            keyboardNumPad.Visible = (pageSelector.CurrentSelection == "Keyboard - NumPad");

            //base.Update(gameTime);
        }
    }
}
