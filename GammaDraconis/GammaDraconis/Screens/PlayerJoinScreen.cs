using System;
using System.Collections.Generic;
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
        private bool[] playersJoined = { false, false, false, false };
        private bool[] playersReady = { false, false, false, false };
        private bool startGame = false;

        private Text[] playerJoinText;
        private Text startGameText;
        private Selector[] shipSelection;

        //bool[] playersJoined = { false, false, false, false };
        /// <summary>
        /// Initialize the player join screen.
        /// </summary>
        /// <param name="game">Reference to the game</param>
        public PlayerJoinScreen(GammaDraconis game)
            : base(game)
        {
            inputs = new PlayerInput[4];

            // Initialize any text or sprite components before adding them to the interface.
            playerJoinText = new Text[4];
            shipSelection = new Selector[4];

            for (int i = 0; i < 4; i++)
            {
                playerJoinText[i] = new Text(game);
                playerJoinText[i].color = Color.White;
                playerJoinText[i].spriteFontName = "Resources/Fonts/Menu";
                playerJoinText[i].center = true;

                shipSelection[i] = new Selector(game, "Raptor", "Ship2", "Ship3");
                shipSelection[i].color = Color.White;
                shipSelection[i].spriteFontName = "Resources/Fonts/Menu";
                shipSelection[i].center = true;
            }

            playerJoinText[0].RelativePosition = new Vector2(1024 / 4, 768 / 4);
            playerJoinText[1].RelativePosition = new Vector2(3 * (1024 / 4), 768 / 4);
            playerJoinText[2].RelativePosition = new Vector2(1024 / 4, 3 * (768 / 4));
            playerJoinText[3].RelativePosition = new Vector2(3 * (1024 / 4), 3 * (768 / 4));

            shipSelection[0].RelativePosition = new Vector2(1024 / 4, 768 / 4);
            shipSelection[1].RelativePosition = new Vector2(3 * (1024 / 4), 768 / 4);
            shipSelection[2].RelativePosition = new Vector2(1024 / 4, 3 * (768 / 4));
            shipSelection[3].RelativePosition = new Vector2(3 * (1024 / 4), 3 * (768 / 4));

            screenInterface.AddComponents(playerJoinText);
            screenInterface.AddComponents(shipSelection);

            startGameText = new Text(game, "Press Start to Begin the Race!");
            startGameText.Visible = false;
            startGameText.color = Color.White;
            startGameText.spriteFontName = "Resources/Fonts/Menu";
            startGameText.center = true;
            startGameText.RelativePosition = new Vector2(1024 / 2, 768 / 2);

            screenInterface.AddComponent(startGameText);
        }

        public override void Initialize()
        {
            GameObject skybox = new GameObject();
            skybox.models.Add(new FBXModel("Resources/Models/Skybox", "", 0.5f));
            screenScene.track(skybox, GO_TYPE.SKYBOX);

            onFreshLoad();

            base.Initialize();
        }

        protected void setUpDummyShips() 
        {
            Player p1 = Player.cloneShip(Proto.getThing("Dummy", new Coords(0f, 0f, 5f, 0f, 0f, 0f)), PlayerIndex.One);
            screenScene.track(p1, GO_TYPE.RACER);

            Player p2 = Player.cloneShip(Proto.getThing("Dummy", new Coords(5f, 0f, 0f, 0f, (float)MathHelper.PiOver4, 0f)), PlayerIndex.Two);
            screenScene.track(p2, GO_TYPE.RACER); 

            Player p3 = Player.cloneShip(Proto.getThing("Dummy", new Coords(0f, 0f, -5f, 0f, (float)MathHelper.PiOver2, 0f)), PlayerIndex.Three);
            screenScene.track(p3, GO_TYPE.RACER);

            Player p4 = Player.cloneShip(Proto.getThing("Dummy", new Coords(-5f, 0f, 0f, 0f, (float)3 * MathHelper.PiOver4, 0f)), PlayerIndex.Four);
            screenScene.track(p4, GO_TYPE.RACER);
        }

        protected override void onFreshLoad()
        {
            // Detect any new controllers
            GammaDraconis.GetInstance().InputManager.AutoRegisterControlSchemes();

            inputs[0] = GammaDraconis.GetInstance().InputManager.GetPlayerInput(PlayerIndex.One);
            inputs[1] = GammaDraconis.GetInstance().InputManager.GetPlayerInput(PlayerIndex.Two);
            inputs[2] = GammaDraconis.GetInstance().InputManager.GetPlayerInput(PlayerIndex.Three);
            inputs[3] = GammaDraconis.GetInstance().InputManager.GetPlayerInput(PlayerIndex.Four);

            // Reset state variables
            for (int i = 0; i < inputs.Length; i++)
            {
                playersJoined[i] = false;
                playersReady[i] = false;
            }
            startGame = false;

            setUpDummyShips();
        }

        public override void Update(GameTime gameTime)
        {          
            for( int index = 0; index < inputs.Length; index++ )
            {
                if (!playersJoined[index])
                {
                    // Player has not joined game.

                    try
                    {
                        playerJoinText[index].text = "Press " + inputs[index].getKeyBinding(PlayerInput.Commands.Join) + " to Join Game";

                        if (inputs[index].inputPressed(PlayerInput.Commands.Join))
                        {
                            playersJoined[index] = true;
                            startGameText.Visible = false;
                            startGame = false;
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
                        shipSelection[index].PrevSelection();
                    }
                    else if (inputs[index].inputPressed(PlayerInput.Commands.MenuRight))
                    {
                        shipSelection[index].NextSelection();
                    }
                }
                else
                {
                    if (inputs[index].inputPressed(PlayerInput.Commands.Leave))
                    {
                        // If the player is ready, return them to ship selection.
                        playersReady[index] = false;
                        startGameText.Visible = false;
                    }
                    else if (inputs[index].inputPressed(PlayerInput.Commands.GameStart))
                    {
                        // If this player is ready, start the game.
                        if (playersReady[index])
                            startGame = true;
                    }
                }

                playerJoinText[index].Visible = !playersJoined[index];
                shipSelection[index].Visible = playersJoined[index];
            }

            // Check for game start conditions.
            if (((playersJoined[0] == true) || (playersJoined[1] == true) || (playersJoined[2] == true) || (playersJoined[3] == true)) &&
                (playersJoined[0] == playersReady[0]) && (playersJoined[1] == playersReady[1]) && 
                (playersJoined[2] == playersReady[2]) && (playersJoined[3] == playersReady[3]))
            {
                startGameText.Visible = true;

                if (startGame)
                {
                    GameObject ship = Proto.getShip("Raptor");
                    List<Player> players = new List<Player>();
                    for (int index = 0; index < inputs.Length; index++)
                    {
                        if (playersJoined[index])
                        {

                            players.Add(Player.cloneShip(ship, (PlayerIndex)index));
                        }
                        else
                        {
                            Player.players[index] = null;
                        }
                    }

                    ((GameScreen)gammaDraconis.getScreen(GammaDraconis.GameStates.Game)).ReloadEngine("CircleTrack", players);

                    gammaDraconis.changeState(GammaDraconis.GameStates.GameLoading);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GammaDraconis.renderer.render(gameTime, screenScene, false);
            base.Draw(gameTime, false);
        }
    }
}
