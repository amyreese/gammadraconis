using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Types;
using GammaDraconis.Video;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Screens.Menus
{
    class ControlsMenu : MenuScreen
    {
        private GameObject skybox;

        public ControlsMenu(GammaDraconis game)
            : base(game)
        {
            skybox = new Skybox();
            screenScene.track(skybox, GO_TYPE.SKYBOX);

            Sprite xbox360PadFront = new Sprite(game);
            xbox360PadFront.textureName = "Resources/Textures/Xbox 360 Controller Front";
            xbox360PadFront.RelativePosition = new Vector2(112.0f, 100.0f);
            screenInterface.AddComponent(xbox360PadFront);

            Sprite xbox360PadTop = new Sprite(game);
            xbox360PadTop.textureName = "Resources/Textures/Xbox 360 Controller Top";
            xbox360PadTop.RelativePosition = new Vector2(112.0f, 480.0f);
            screenInterface.AddComponent(xbox360PadTop);
        }

        /// <summary>
        /// The commands that can be used for this menu
        /// </summary>
        private class Commands
        {
            public static string Back = "Back";
        }

        protected override void SetupMenuItems()
        {
            menuItems = new MenuItem[1];
            menuItems[0] = new MenuItem(gammaDraconis, Commands.Back);
            menuItems[0].text = "";
        }

        protected override string GetBackgroundImageName()
        {
            return null;
        }

        protected override void ItemSelected(string command)
        {
            gammaDraconis.changeState(GammaDraconis.GameStates.MainMenu);
        }

        protected override void Cancel()
        {
            ItemSelected(Commands.Back);
        }
    }
}
