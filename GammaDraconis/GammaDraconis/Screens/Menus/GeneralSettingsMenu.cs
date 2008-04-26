using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Core;
using GammaDraconis.Video.GUI;
using GammaDraconis.Types;
using GammaDraconis.Video;

namespace GammaDraconis.Screens.Menus
{
    class GeneralSettingsMenu : MenuScreen
    {
        Racer racer;
        GameObject skybox;
		private Vector3 startLocation = new Vector3(120.0f, -4.35f, -100.0f);
		private int timeToStartIndex;
        private int useMouseIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public GeneralSettingsMenu(GammaDraconis game)
            : base(game)
        {
			skybox = new GameObject();
			skybox.models.Add(new FBXModel("Resources/Models/Skybox", "", 0.195f));
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
            public static string ToggleTimeToStart = "ToggleTimeToStart";
            public static string ToggleUseMouse = "ToggleUseMouse";
            public static string Back = "Back";
        }

        /// <summary>
        /// Sets up the menu items for the main menu
        /// </summary>
        protected override void SetupMenuItems()
        {
            Interface menuRegion = new Interface(gammaDraconis);
            menuRegion.RelativePosition = new Vector2(100.0f, Game.Window.ClientBounds.Height / 1.6f);
            screenInterface.AddComponent(menuRegion);

            menuItems = new MenuItem[3];
            menuItems[0] = new MenuItem(gammaDraconis, Commands.ToggleTimeToStart);
            menuItems[0].text = "Toggle Time To Start";
            timeToStartIndex = 0;
            menuItems[1] = new MenuItem(gammaDraconis, Commands.ToggleUseMouse);
            menuItems[1].text = "Toggle Use Mouse";
            useMouseIndex = 1;
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
            if (command.Equals(Commands.ToggleTimeToStart))
            {
                Properties.Settings.Default.RaceStartDelay++;
                if (Properties.Settings.Default.RaceStartDelay > 10)
                {
                    Properties.Settings.Default.RaceStartDelay = 0;
                }
                Properties.Settings.Default.Save();
            }
            else if (command.Equals(Commands.ToggleUseMouse))
            {
                Properties.Settings.Default.PlayerOneUseMouse = !Properties.Settings.Default.PlayerOneUseMouse;
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
            menuItems[timeToStartIndex].text = "Race Start Delay: " + Properties.Settings.Default.RaceStartDelay + " seconds";
            menuItems[useMouseIndex].text = "Use Mouse Navigation: " + Properties.Settings.Default.PlayerOneUseMouse;

			/*
			racer.position.T *= Matrix.CreateTranslation((float)(-25f * gameTime.ElapsedGameTime.TotalSeconds), (float)(5f * gameTime.ElapsedGameTime.TotalSeconds), 0);
			if (racer.position.pos().X < -120)
			{
				racer.position.T = Matrix.CreateTranslation(startLocation);
			}
			 */
            base.Update(gameTime);
        }
    }
}
