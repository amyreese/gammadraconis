using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Core;
using GammaDraconis.Core.Input;
using GammaDraconis.Types;
using GammaDraconis.Video;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// Screen giving players the chance to join the race, 
    /// select their ship, and select their weapons.
    /// </summary>
    class PlayerJoinScreen : Screen
    {
        PlayerInput[] inputs;
        bool[] playersJoined = { true, true, true, true };

        /// <summary>
        /// Initialize the player join screen.
        /// </summary>
        /// <param name="game">Reference to the game</param>
        public PlayerJoinScreen(GammaDraconis game)
            : base(game)
        {
            inputs = new PlayerInput[4];
        }

        public override void Initialize()
        {
            inputs[0] = new PlayerInput(PlayerIndex.One);
            inputs[1] = new PlayerInput(PlayerIndex.Two);
            inputs[2] = new PlayerInput(PlayerIndex.Three);
            inputs[3] = new PlayerInput(PlayerIndex.Four);

            GameObject skybox = new GameObject();
            skybox.models.Add(new FBXModel("Resources/Models/Skybox", "", 0.5f));
            screenScene.track(skybox, GO_TYPE.SKYBOX);

            Player p1 = Player.cloneShip(Proto.getThing("Dummy", new Coords(0f, 0f, 5f, 0f, 0f, 0f)), PlayerIndex.One);
            screenScene.track(p1, GO_TYPE.RACER);

            Player p2 = Player.cloneShip(Proto.getThing("Dummy", new Coords(5f, 0f, 0f, 0f, (float)MathHelper.PiOver4, 0f)), PlayerIndex.Two);
            screenScene.track(p2, GO_TYPE.RACER); 

            Player p3 = Player.cloneShip(Proto.getThing("Dummy", new Coords(0f, 0f, -5f, 0f, (float)MathHelper.PiOver2, 0f)), PlayerIndex.Three);
            screenScene.track(p3, GO_TYPE.RACER);

            Player p4 = Player.cloneShip(Proto.getThing("Dummy", new Coords(-5f, 0f, 0f, 0f, (float)3 * MathHelper.PiOver4, 0f)), PlayerIndex.Four);
            screenScene.track(p4, GO_TYPE.RACER);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (input.inputPressed(Core.Input.MenuInput.Commands.Cancel))
            {
                gammaDraconis.changeState(GammaDraconis.GameStates.MainMenu);
            }

            /*
            for( int index = 0; index < inputs.Length; index ++ )
            {
                if (inputs[index].inputPressed(PlayerInput.Commands.Join))
                {
                    playersJoined[index] = true;
                }
                if (inputs[index].inputPressed(PlayerInput.Commands.Leave))
                {
                    playersJoined[index] = false;
                }
            }
            */

            if (input.inputPressed(MenuInput.Commands.Select))
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

                ((GameScreen)gammaDraconis.getScreen(GammaDraconis.GameStates.Game)).ReloadEngine("CircleTrack", players );


                gammaDraconis.changeState(GammaDraconis.GameStates.GameLoading);
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
