using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Core;
using GammaDraconis.Core.Input;
using GammaDraconis.Types;
using GammaDraconis.Video;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// Screen giving players the chance to join the race, 
    /// select their ship, and select their weapons.
    /// </summary>
    class PlayerJoinScreen : Screen
    {
        private PlayerInput[] inputs;
        private Dictionary<string, string> keyGlyphs;

        // State variables
        private bool[] playersJoined = { false, false, false, false };
        private bool[] playersReady = { false, false, false, false };
        private bool startGame = false;

        // GUI components
        private Text[] playerJoinText;
        private Text[] playerJoinGlyph;
        private Text startGameText;
        private Selector trackSelector;
        private Selector[] shipSelector;
        private GameObject[] selectedShip;
        private Coords[] playerCoords;
        private Coords[] shipCoords;

        /// <summary>
        /// Initialize the player join screen.
        /// </summary>
        /// <param name="game">Reference to the game</param>
        public PlayerJoinScreen(GammaDraconis game)
            : base(game)
        {
            inputs = new PlayerInput[4];

            playerCoords = new Coords[4];
            playerCoords[0] = new Coords(  0f, 0f, 0f, 0f, 0f, 0f);
            playerCoords[1] = new Coords(100f, 0f, 0f, 0f, 0f, 0f);
            playerCoords[2] = new Coords(200f, 0f, 0f, 0f, 0f, 0f);
            playerCoords[3] = new Coords(300f, 0f, 0f, 0f, 0f, 0f);

            shipCoords = new Coords[4];
            shipCoords[0] = new Coords(  0f, 0f, -40f, 0.3f, 0f, 0f);
            shipCoords[1] = new Coords(100f, 0f, -40f, 0.3f, 0f, 0f);
            shipCoords[2] = new Coords(200f, 0f, -40f, 0.3f, 0f, 0f);
            shipCoords[3] = new Coords(300f, 0f, -40f, 0.3f, 0f, 0f);

            // Initialize any text or sprite components before adding them to the interface.
            playerJoinText = new Text[4];
            playerJoinGlyph = new Text[4];
            shipSelector = new Selector[4];
            selectedShip = new GameObject[4];

            for (int i = 0; i < 4; i++)
            {
                playerJoinText[i] = new Text(game);
                playerJoinText[i].color = Color.White;
                playerJoinText[i].SpriteFontName = "Resources/Fonts/Menu";
                playerJoinText[i].center = true;

                playerJoinGlyph[i] = new Text(game);
                playerJoinGlyph[i].color = Color.White;
                playerJoinGlyph[i].center = true;

                shipSelector[i] = new Selector(game, Proto.ship.Keys);
                shipSelector[i].color = Color.White;
                shipSelector[i].SpriteFontName = "Resources/Fonts/Menu";
                shipSelector[i].center = true;

                selectedShip[i] = Proto.getShip(shipSelector[i].CurrentSelection, shipCoords[i]);
                screenScene.track(selectedShip[i], GO_TYPE.RACER);
            }

            playerJoinText[0].RelativePosition = new Vector2(1024 / 4, 768 / 4);
            playerJoinText[1].RelativePosition = new Vector2(3 * (1024 / 4), 768 / 4);
            playerJoinText[2].RelativePosition = new Vector2(1024 / 4, 3 * (768 / 4));
            playerJoinText[3].RelativePosition = new Vector2(3 * (1024 / 4), 3 * (768 / 4));
            screenInterface.AddComponents(playerJoinText);

            for(int i = 0; i < 4; i++)
                playerJoinGlyph[i].RelativePosition = playerJoinText[i].RelativePosition;
            //screenInterface.AddComponents(playerJoinGlyph);            

            shipSelector[0].RelativePosition = new Vector2(1024 / 4, (768 + 350) / 4 );
            shipSelector[1].RelativePosition = new Vector2(3 * (1024 / 4), (768 + 350) / 4);
            shipSelector[2].RelativePosition = new Vector2(1024 / 4, (3 * 768 + 350) / 4);
            shipSelector[3].RelativePosition = new Vector2(3 * (1024 / 4), (3 * 768 + 350) / 4);
            screenInterface.AddComponents(shipSelector);

            startGameText = new Text(game, "Press Start to Begin the Race!");
            startGameText.Visible = false;
            startGameText.color = Color.White;
            startGameText.SpriteFontName = "Resources/Fonts/Menu";
            startGameText.center = true;
            startGameText.RelativePosition = new Vector2(1024 / 2, 768 / 2);
            screenInterface.AddComponent(startGameText);

            trackSelector = new Selector(game);
            trackSelector.Visible = false;
            trackSelector.color = Color.White;
            trackSelector.SpriteFontName = "Resources/Fonts/Menu";
            trackSelector.center = true;
            trackSelector.RelativePosition = new Vector2(1024 / 2, 768 / 2);
            screenInterface.AddComponent(trackSelector);

            // Populate keyGlyphs dictionary to map Xbox button assignments to their corresponding spritefont characters.
            keyGlyphs = new Dictionary<string, string>();
            keyGlyphs.Add("PadBack", "#");
            keyGlyphs.Add("PadStart", "%");
            keyGlyphs.Add("PadX", "&");
            keyGlyphs.Add("PadA", "'");
            keyGlyphs.Add("PadY", "(");
            keyGlyphs.Add("PadB", ")");
            keyGlyphs.Add("PadRB", "*");
            keyGlyphs.Add("PadLB", "-");
        }

        public override void Initialize()
        {
            GameObject skybox = new Skybox();
            screenScene.track(skybox, GO_TYPE.SKYBOX);

            onFreshLoad();

            base.Initialize();
        }

        protected void setUpDummyShips() 
        {
            Player p1 = Proto.getPlayer("Dummy", PlayerIndex.One, playerCoords[0]);
            screenScene.track(p1, GO_TYPE.RACER);

            Player p2 = Proto.getPlayer("Dummy", PlayerIndex.Two, playerCoords[1]);
            screenScene.track(p2, GO_TYPE.RACER);

            Player p3 = Proto.getPlayer("Dummy", PlayerIndex.Three, playerCoords[2]);
            screenScene.track(p3, GO_TYPE.RACER);

            Player p4 = Proto.getPlayer("Dummy", PlayerIndex.Four, playerCoords[3]);
            screenScene.track(p4, GO_TYPE.RACER);
        }

        protected override void onFreshLoad()
        {
            setUpDummyShips();

            // Detect any new controllers
            GammaDraconis.GetInstance().InputManager.AutoRegisterControlSchemes();

            inputs[0] = GammaDraconis.GetInstance().InputManager.GetPlayerInput(PlayerIndex.One);
            inputs[1] = GammaDraconis.GetInstance().InputManager.GetPlayerInput(PlayerIndex.Two);
            inputs[2] = GammaDraconis.GetInstance().InputManager.GetPlayerInput(PlayerIndex.Three);
            inputs[3] = GammaDraconis.GetInstance().InputManager.GetPlayerInput(PlayerIndex.Four);

            for (int i = 0; i < 4; i++)
            {
                playerJoinText[i].text = "Press " + inputs[i].getKeyBinding(PlayerInput.Commands.Join) + " to Join Game";

                if (inputs[i].ControlScheme == InputManager.ControlScheme.GamePad)
                    playerJoinGlyph[i].SpriteFontName = "Resources/Fonts/XboxController";
                else
                    playerJoinGlyph[i].SpriteFontName = "Resources/Fonts/Menu";

                playerJoinGlyph[i].text = GetKeyGlyph(inputs[i].getKeyBinding(PlayerInput.Commands.Join));
            }

            // Reset state variables
            for (int i = 0; i < inputs.Length; i++)
            {
                playersJoined[i] = false;
                playersReady[i] = false;
            }
            startGame = false;

            // Load list of maps.
            if (Directory.Exists("Maps"))
            {
                string[] maps = Directory.GetDirectories("Maps");

                foreach (string map in maps)
                {
                    trackSelector.AddSelection(map.Substring(map.IndexOf("\\") + 1));
                }
            }

            
        }

        public override void Update(GameTime gameTime)
        {
            // Update must be called on the base class first in order call onFreshLoad, if necessary.
            base.Update(gameTime);

            // Handle input from all players.
            for( int index = 0; index < inputs.Length; index++ )
            {
                if (!playersJoined[index])
                {
                    // Player has not joined game.

                    screenScene.ignore(selectedShip[index]);

                    try
                    {
                        if (inputs[index].inputPressed(PlayerInput.Commands.Join))
                        {
                            playersJoined[index] = true;
                            startGame = false;

                            screenScene.track(selectedShip[index], GO_TYPE.RACER);
                        }
                    }
                    catch
                    {
                        playerJoinText[index].text = "Connect Controller";
                    }

                    if (inputs[index].inputPressed(PlayerInput.Commands.Menu))
                    {
                        gammaDraconis.changeState(GammaDraconis.GameStates.MainMenu);
                    }
                }
                else if (!playersReady[index])
                {
                    // Player has joined game, but isn't ready

                    if (inputs[index].inputPressed(PlayerInput.Commands.Leave))
                    {
                        // The player leaves the game.
                        playersJoined[index] = false;
                    }
                    else if (inputs[index].inputPressed(PlayerInput.Commands.Join))
                    {
                        // Make the player ready.
                        playersReady[index] = true;
                    }
                    else if (inputs[index].inputPressed(PlayerInput.Commands.MenuLeft))
                    {
                        shipSelector[index].PrevSelection();

                        screenScene.ignore(selectedShip[index]);
                        selectedShip[index] = Proto.getShip(shipSelector[index].CurrentSelection, shipCoords[index]);
                        screenScene.track(selectedShip[index], GO_TYPE.RACER);
                    }
                    else if (inputs[index].inputPressed(PlayerInput.Commands.MenuRight))
                    {
                        shipSelector[index].NextSelection();

                        screenScene.ignore(selectedShip[index]);
                        selectedShip[index] = Proto.getShip(shipSelector[index].CurrentSelection, shipCoords[index]);
                        screenScene.track(selectedShip[index], GO_TYPE.RACER);
                    }
                }
                else
                {
                    if (inputs[index].inputPressed(PlayerInput.Commands.Leave))
                    {
                        // If the player is ready, return them to ship selection.
                        playersReady[index] = false;
                        trackSelector.Visible = false;
                    }
                    else if (inputs[index].inputPressed(PlayerInput.Commands.GameStart) ||
                        inputs[index].inputPressed(PlayerInput.Commands.Join))
                    {
                        // If this player is ready, start the game.
                        if (playersReady[index])
                            startGame = true;
                    }
                    else if (inputs[index].inputPressed(PlayerInput.Commands.MenuLeft))
                    {
                        trackSelector.PrevSelection();
                    }
                    else if (inputs[index].inputPressed(PlayerInput.Commands.MenuRight))
                    {
                        trackSelector.NextSelection();
                    }
                }

                playerJoinText[index].Visible = !playersJoined[index];
                playerJoinGlyph[index].Visible = !playersJoined[index];
                shipSelector[index].Visible = playersJoined[index];
                trackSelector.Visible = startGame;
                selectedShip[index].position.R *= Quaternion.CreateFromYawPitchRoll((float)Math.PI / 60f, 0f, 0f);
            }

            // Check for game start conditions.
            if (((playersJoined[0] == true) || (playersJoined[1] == true) || (playersJoined[2] == true) || (playersJoined[3] == true)) &&
                (playersJoined[0] == playersReady[0]) && (playersJoined[1] == playersReady[1]) && 
                (playersJoined[2] == playersReady[2]) && (playersJoined[3] == playersReady[3]))
            {
                trackSelector.Visible = true;

                if (startGame)
                {
                    // Set up game objects and begin the game.
                    GameObject ship;
                    List<Player> players = new List<Player>();
                    for (int index = 0; index < inputs.Length; index++)
                    {
                        if (playersJoined[index])
                        {
                            ship = Proto.getShip(shipSelector[index].CurrentSelection);
                            players.Add(Player.cloneShip(ship, (PlayerIndex)index));
                        }
                        else
                        {
                            Player.players[index] = null;
                        }
                    }

                    ((GameScreen)gammaDraconis.getScreen(GammaDraconis.GameStates.Game)).ReloadEngine(trackSelector.CurrentSelection, players);

                    gammaDraconis.changeState(GammaDraconis.GameStates.GameLoading);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GammaDraconis.renderer.render(gameTime, screenScene, false);
            base.Draw(gameTime, false);
        }

        private string GetKeyGlyph(string button)
        {
            if (keyGlyphs.ContainsKey(button))
                return keyGlyphs[button];
            return button;
        }
    }
}
