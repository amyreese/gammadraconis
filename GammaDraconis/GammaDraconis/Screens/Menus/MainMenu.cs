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
    class MainMenu : MenuScreen
    {
        Racer racer;
        GameObject skybox;
        private Vector3 startLocation = new Vector3(120.0f, -4.35f, -100.0f);

        /// <summary>
        /// Create the main menu.
        /// </summary>
        /// <param name="game">The game instance.</param>
        public MainMenu(GammaDraconis game)
            : base(game)
        {
            skybox = new Skybox();
            screenScene.track(skybox, GO_TYPE.SKYBOX);
            Skybox.lights[0] = new Light(new Vector3(-0.05f,  0.1f, -1f), new Vector3(0.9f, 0.7f, 0.7f), new Vector3(1f,1f,1f));
            Skybox.lights[1] = new Light(new Vector3(0.95f, -0.9f, 1f), new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0.5f, 0.5f, 0.5f));

            if((new Random()).Next(1, 3) == 1)
                racer = Proto.getRacer("Raptor");
            else
                racer = Proto.getRacer("Thor");
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
            public static string Play = "Play";
            public static string Controls = "Controls";
            public static string GeneralSettings = "GeneralSettings";
            public static string VideoSettings = "VideoSettings";
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

            menuItems = new MenuItem[6];
            menuItems[0] = new MenuItem(gammaDraconis, Commands.Play);
            menuItems[0].text = "Play";
            menuItems[1] = new MenuItem(gammaDraconis, Commands.Controls);
            menuItems[1].text = "Controls";
            menuItems[2] = new MenuItem(gammaDraconis, Commands.GeneralSettings);
            menuItems[2].text = "Gameplay Settings";
            menuItems[3] = new MenuItem(gammaDraconis, Commands.VideoSettings);
            menuItems[3].text = "Video Settings";
            menuItems[4] = new MenuItem(gammaDraconis, Commands.ToggleFullscreen);
            menuItems[4].text = "Toggle Fullscreen";
            menuItems[5] = new MenuItem(gammaDraconis, Commands.Quit);
            menuItems[5].text = "Quit";

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
            if (command.Equals(Commands.Play))
            {
                if (!gammaDraconis.GameStarted)
                {
                    gammaDraconis.changeState(GammaDraconis.GameStates.PlayerJoin);
                }
                else
                {
                    gammaDraconis.changeState(GammaDraconis.GameStates.GameLoading);
                }
            }
            else if (command.Equals(Commands.Quit))
            {
                Game.Exit();
            }
            else if (command.Equals(Commands.ToggleFullscreen))
            {
                gammaDraconis.ToggleFullscreen();
            }
            else if (command.Equals(Commands.GeneralSettings))
            {
                gammaDraconis.changeState(GammaDraconis.GameStates.GeneralSettingsMenu);
            }
            else if (command.Equals(Commands.VideoSettings))
            {
                gammaDraconis.changeState(GammaDraconis.GameStates.VideoSettingsMenu);
            }
            else if (command.Equals(Commands.Controls))
            {
                gammaDraconis.changeState(GammaDraconis.GameStates.KeyBindingsMenu);
            }
            else
            {
                Console.WriteLine("Not yet implemented!");
            }
        }

        protected override void Cancel()
        {
            //ItemSelected(Commands.Play);
        }

        public override void Update(GameTime gameTime)
        {
            racer.position.T *= Matrix.CreateTranslation((float)(-25f * gameTime.ElapsedGameTime.TotalSeconds), (float)(5f * gameTime.ElapsedGameTime.TotalSeconds), 0);
            if (racer.position.pos().X < -120)
            {
                racer.position.T = Matrix.CreateTranslation(startLocation);
            }
            base.Update(gameTime);
        }
    }
}
