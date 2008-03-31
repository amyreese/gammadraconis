using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Core.Input;
using GammaDraconis.Video;
using GammaDraconis.Video.GUI;


namespace GammaDraconis.Types
{
    /// <summary>
    /// A Player is a special racer that is controlled by a human.
    /// </summary>
    class Player : Racer
    {
        public static Player[] players = new Player[4];

        public PlayerIndex index;
        public PlayerInput input;
        public Coords camera;
        public Renderer.Viewports viewport;

        public Interface playerHUD;

        public Player(GammaDraconis game, PlayerIndex index)
            : base( game )
        {
            this.index = index;

            input = new PlayerInput(index);
            camera = new Coords();
            viewport = (Renderer.Viewports)index;
            Player.players[(int)index] = this;

            playerHUD = (Interface)GammaDraconis.GetInstance().GameLua.DoString("playerHudIndex = " + ((int)index + 1) + "\nreturn dofile( 'Interfaces/PlayerHUD/PlayerHUD.lua' )")[0];
        }

        public override void think(GameTime gameTime)
        {
            playerHUD.Update(gameTime);

            #region Keyboard input handling
            if (input.inputDown("Up"))
            {
                pitch(1);
            }
            if (input.inputDown("Down"))
            {
                pitch(-1);
            }
            if (input.inputDown("Left"))
            {
                turn(-1);
            }
            if (input.inputDown("Right"))
            {
                turn(1);
            }
            if (input.inputDown("RollLeft"))
            {
                roll(-1);
            }
            if (input.inputDown("RollRight"))
            {
                roll(1);
            }

            if (input.inputDown("ThrottleUp"))
            {
                throttle(1);
            }
            if (input.inputDown("ThrottleDown"))
            {
                throttle(-1);
            }
            #endregion

            #region Gamepad input handling
            {
                pitch(input.axis("Pitch"));
                turn(input.axis("Turn"));
                roll(input.axis("Roll"));
                yaw(input.axis("Yaw"));

                throttle(input.axis("Throttle"));

                // Rotate the camera around the player
                camera.R = Quaternion.CreateFromYawPitchRoll((float)Math.PI * input.axis("CameraX"), (float)Math.PI * -input.axis("CameraY"), 0f);
            }
            #endregion
        }

        public Matrix getCameraLookAtMatrix()
        {
            Coords c = new Coords();
            c.R = position.R * camera.R;
            c.T = Matrix.CreateTranslation(0f, 1f, 4f) * Matrix.CreateFromQuaternion(camera.R) * Matrix.CreateFromQuaternion(position.R) * position.T;
            Matrix m = Matrix.CreateTranslation(0f, 1f, -10f) * Matrix.CreateFromQuaternion(camera.R) * Matrix.CreateFromQuaternion(position.R) * position.T;
            return Matrix.CreateLookAt(c.pos(), m.Translation, c.up());
        }
    }
}
 