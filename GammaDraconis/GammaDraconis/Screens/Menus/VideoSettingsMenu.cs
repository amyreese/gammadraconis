using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Core;
using GammaDraconis.Video.GUI;
using GammaDraconis.Types;
using GammaDraconis.Video;
using GammaDraconis.Video.Shaders;

namespace GammaDraconis.Screens.Menus
{
    class VideoSettingsMenu : MenuScreen
    {
        Racer racer;
        GameObject skybox;
        private Vector3 startLocation = new Vector3(20.0f, -1.35f, -12.0f);
        private int bloomIndex;
        private int perPixelLightingIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public VideoSettingsMenu(GammaDraconis game)
            : base(game)
        {
            skybox = new GameObject();
            skybox.models.Add(new FBXModel("Resources/Models/Skybox", "", 0.05f));
            screenScene.track(skybox, GO_TYPE.SKYBOX);


            racer = Proto.getRacer("Raptor");
            racer.position = new Coords(startLocation.X, startLocation.Y, startLocation.Z, 0.2f, 1.5f, 1.0f);
            racer.models[0].scale *= 1;
            racer.size *= 1;
            screenScene.track(racer, GO_TYPE.RACER);

            GameObject planet = new GameObject();
            planet.position = new Coords(500f, -300f, -1250f);
            planet.models.Add(new FBXModel("Resources/Models/Planet", "", 1f));
            screenScene.track(planet, GO_TYPE.SCENERY);

            Text NameText = new Text(game);
            NameText.text = "Gamma Draconis";
            NameText.RelativeRotation = 0.2f;
            NameText.RelativePosition = new Vector2(300.0f, 8.0f);
            NameText.spriteFontName = "Resources/Fonts/Title";
            NameText.color = Color.White;
            screenInterface.AddComponent(NameText);
        }

        /// <summary>
        /// The commands that can be used for this menu
        /// </summary>
        private class Commands
        {
            public static string ToggleBloom = "ToggleBloom";
            public static string TogglePPL = "TogglePPL";
            public static string Back = "Back";
        }

        /// <summary>
        /// Sets up the menu items for the main menu
        /// </summary>
        protected override void SetupMenuItems()
        {
            Interface menuRegion = new Interface(gammaDraconis);
            menuRegion.RelativePosition = new Vector2(100.0f, Game.Window.ClientBounds.Height / 1.8f);
            screenInterface.AddComponent(menuRegion);

            menuItems = new MenuItem[3];
            menuItems[0] = new MenuItem(gammaDraconis, Commands.ToggleBloom);
            menuItems[0].text = "Toggle Bloom";
            bloomIndex = 0;
            menuItems[1] = new MenuItem(gammaDraconis, Commands.TogglePPL);
            menuItems[1].text = "Toggle PPL";
            perPixelLightingIndex = 1;
            menuItems[2] = new MenuItem(gammaDraconis, Commands.Back);
            menuItems[2].text = "Back";

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
            return null;
        }

        /// <summary>
        /// Activates a menu item
        /// </summary>
        /// <param name="item">The menu item selected</param>
        protected override void ItemSelected(String command)
        {
            if (command.Equals(Commands.ToggleBloom))
            {
                int nextIndex = ((Bloom)GammaDraconis.renderer.shaders["bloom"]).Settings.Index + 1;
                if (nextIndex == BloomSettings.PresetSettings.Length)
                {
                    nextIndex = 0;
                }
                ((Bloom)GammaDraconis.renderer.shaders["bloom"]).Settings = BloomSettings.PresetSettings[nextIndex];
                Properties.Settings.Default.BloomSetting = nextIndex;
                Properties.Settings.Default.Save();
            }
            else if (command.Equals(Commands.TogglePPL))
            {
                Properties.Settings.Default.PerPixelLighting = !Properties.Settings.Default.PerPixelLighting;
                Properties.Settings.Default.Save();
            }
            else if (command.Equals(Commands.Back))
            {
                gammaDraconis.changeState(GammaDraconis.GameStates.MainMenu);
            }
            else
            {
                Console.WriteLine("Not yet implemented!");
            }
        }

        protected override void Cancel()
        {
            ItemSelected(Commands.Back);
        }

        public override void Update(GameTime gameTime)
        {
            menuItems[bloomIndex].text = "Bloom: " + ((Bloom)GammaDraconis.renderer.shaders["bloom"]).Settings.Name;
            menuItems[perPixelLightingIndex].text = "Per Pixel Lighting: " + (Properties.Settings.Default.PerPixelLighting ? "Yes" : "No");

            /*
            racer.position.T *= Matrix.CreateTranslation((float)(-7.5f * gameTime.ElapsedGameTime.TotalSeconds), (float)(1.5f * gameTime.ElapsedGameTime.TotalSeconds), 0);
            if (racer.position.pos().X < -20)
            {
                racer.position.T = Matrix.CreateTranslation(startLocation);
            }
             * */
            base.Update(gameTime);
        }
    }
}
