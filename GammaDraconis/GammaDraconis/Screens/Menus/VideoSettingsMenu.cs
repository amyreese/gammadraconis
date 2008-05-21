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
		private Vector3 startLocation = new Vector3(120.0f, -4.35f, -100.0f);
        private int bloomIndex;
		private int bloomTypeIndex;
        private int perPixelLightingIndex;
        private int resolutionIndex;
        private int currentResolutionIndex;
        private string[] resolutionList = { "800x600", "1024x768", "1280x1024", "1600x1200" };

        /// <summary>
        /// Create this menu.
        /// </summary>
        /// <param name="game">The game instance.</param>
        public VideoSettingsMenu(GammaDraconis game)
            : base(game)
        {
			skybox = new Skybox();
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

            Sprite NameText = new Sprite(game);
            NameText.textureName = "Resources/Textures/Logo";
            NameText.RelativeRotation = 0.2f;
            NameText.RelativePosition = new Vector2(300.0f, 20.0f);
            screenInterface.AddComponent(NameText);
        }

        /// <summary>
        /// The commands that can be used for this menu
        /// </summary>
        private class Commands
        {
            public static string ToggleBloom = "ToggleBloom";
            public static string ToggleBloomType = "ToggleBloomType";
            public static string TogglePPL = "TogglePPL";
            public static string ToggleResolution = "ToggleResolution";
            public static string Back = "Back";
        }

        /// <summary>
        /// Sets up the menu items for the main menu
        /// </summary>
        protected override void SetupMenuItems()
        {
            menuItems = new MenuItem[5];
            menuItems[0] = new MenuItem(gammaDraconis, Commands.ToggleBloom);
            menuItems[0].text = "Bloom Enabled";
            bloomIndex = 0;
            menuItems[1] = new MenuItem(gammaDraconis, Commands.ToggleBloomType);
            menuItems[1].text = "Bloom Type";
            bloomTypeIndex = 1;
            menuItems[2] = new MenuItem(gammaDraconis, Commands.TogglePPL);
            menuItems[2].text = "Toggle PPL";
            perPixelLightingIndex = 2;
            menuItems[3] = new MenuItem(gammaDraconis, Commands.ToggleResolution);
            menuItems[3].text = "Display Resolution";
            resolutionIndex = 3;
            menuItems[4] = new MenuItem(gammaDraconis, Commands.Back);
            menuItems[4].text = "Back";

            AutoPositionMenuItems();

            foreach(MenuItem item in menuItems)
                item.RelativePosition += new Vector2(100.0f, 350.0f);

            screenInterface.AddComponents(menuItems);
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
                bool enabled = !GammaDraconis.renderer.enableShaders;
                GammaDraconis.renderer.enableShaders = enabled;
                Properties.Settings.Default.BloomEnabled = enabled;
                Properties.Settings.Default.Save();
            }
            else if (command.Equals(Commands.ToggleBloomType))
            {
                int nextIndex = GammaDraconis.renderer.bloomShader.Settings.Index + 1;
                if (nextIndex == BloomSettings.PresetSettings.Length)
                {
                    nextIndex = 0;
                }
                GammaDraconis.renderer.bloomShader.Settings = BloomSettings.PresetSettings[nextIndex];
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
            else if (command.Equals(Commands.ToggleResolution))
            {
                if (++currentResolutionIndex == resolutionList.Length)
                    currentResolutionIndex = 0;

                Properties.Settings.Default.HorizontalResolution = int.Parse(resolutionList[currentResolutionIndex].Split('x')[0]);
                Properties.Settings.Default.VerticalResolution = int.Parse(resolutionList[currentResolutionIndex].Split('x')[1]);

                gammaDraconis.graphics.PreferredBackBufferWidth = Properties.Settings.Default.HorizontalResolution;
                gammaDraconis.graphics.PreferredBackBufferHeight = Properties.Settings.Default.VerticalResolution;
                gammaDraconis.graphics.ApplyChanges();
                GammaDraconis.renderer.reset();
            }
            else
            {
                Console.WriteLine("Not yet implemented!");
            }
        }

        /// <summary>
        /// Exit this menu.
        /// </summary>
        protected override void Cancel()
        {
            ItemSelected(Commands.Back);
        }

        /// <summary>
        /// Update what values the menu items display.
        /// </summary>
        /// <param name="gameTime">The current time.</param>
        public override void Update(GameTime gameTime)
        {
            menuItems[bloomIndex].text = "Bloom: " + (GammaDraconis.renderer.enableShaders ? "Enabled" : "Disabled");
            menuItems[bloomTypeIndex].text = "Bloom Type: " + GammaDraconis.renderer.bloomShader.Settings.Name;
            menuItems[perPixelLightingIndex].text = "Per Pixel Lighting: " + (Properties.Settings.Default.PerPixelLighting ? "Yes" : "No");
            menuItems[resolutionIndex].text = "Display Resolution: " + Properties.Settings.Default.HorizontalResolution + "x" + Properties.Settings.Default.VerticalResolution;

            base.Update(gameTime);
        }

        /// <summary>
        /// Reset the window to it's default state.
        /// </summary>
        protected override void onFreshLoad()
        {
            for (currentResolutionIndex = 0; currentResolutionIndex < resolutionList.Length; currentResolutionIndex++)
                if (Properties.Settings.Default.HorizontalResolution <= int.Parse(resolutionList[currentResolutionIndex].Split('x')[0]))
                    break;
        }
    }
}
