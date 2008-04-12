using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Core;
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

        double invulnerabilityTimer = 0;

        public Interface playerHUD;

        public Player(PlayerIndex index)
            : base()
        {
            this.index = index;

            input = new PlayerInput(index);
            camera = new Coords();
            viewport = (Renderer.Viewports)index;
            maxHealth = 100;
            health = maxHealth;
            Player.players[(int)index] = this;

            playerHUD = (Interface)GammaDraconis.GetInstance().GameLua.DoString("playerHudIndex = " + ((int)index + 1) + "\nreturn dofile( 'Interfaces/PlayerHUD/PlayerHUD.lua' )")[0];
        }

        public override void think(GameTime gameTime)
        {
            playerHUD.Update(gameTime);

            #region Death handling
            if (health <= 0)
            {
                position = Engine.GetInstance().race.coord(this, 0);
                velocity = new Coords();
                health = maxHealth;
                invulnerabilityTimer = 2 + gameTime.ElapsedRealTime.TotalSeconds;
            }
            #endregion

            #region Invulnerability Timer
            if (invulnerabilityTimer > 0)
            {
                invincible = true;
                invulnerabilityTimer -= gameTime.ElapsedRealTime.TotalSeconds;
            }
            else
            {
                invincible = false;
            }
            #endregion

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
                yaw(-1);
            }
            if (input.inputDown("Right"))
            {
                yaw(1);
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

            #region Controller-independent handling
            {
                if (input.inputDown("Fire1"))
                {
                    fire();
                }
            }
            #endregion
        }

        public Matrix getCameraLookAtMatrix()
        {
            Coords c = getCamera();
            Matrix m = Matrix.CreateTranslation(0f, 1f, -40f) * Matrix.CreateFromQuaternion(camera.R) * Matrix.CreateFromQuaternion(position.R) * position.T;
            return Matrix.CreateLookAt(c.pos(), m.Translation, c.up());
        }

        public Coords getCamera()
        {
            Coords c = new Coords();
            c.R = position.R * camera.R;
            c.T = Matrix.CreateTranslation(0f, 1f, 3.5f) * Matrix.CreateFromQuaternion(camera.R) * Matrix.CreateFromQuaternion(position.R) * position.T;
            return c;
        }

        public override string ToString()
        {
            return "Player " + index;
        }
        
        /// <summary>
        /// Create a Player object from a ship definition.
        /// </summary>
        /// <param name="ship">The target ship</param>
        /// <param name="index">Player index</param>
        /// <returns>New player object</returns>
        public static Player cloneShip(GameObject ship, PlayerIndex index)
        {
            Player go = new Player(index);

            go.mass = ship.mass;
            go.size = ship.size;

            go.rateL = ship.rateL;
            go.rateR = ship.rateR;
            go.dragL = ship.dragL;
            go.dragR = ship.dragR;

            foreach (FBXModel model in ship.models)
            {
                go.models.Add(model.clone());
            }

            if (ship.shieldModel != null)
            {
                go.shieldModel = ship.shieldModel.clone();
            }
            
            foreach(MountPoint mount in ship.mounts)
            {
                go.mounts.Add(mount.clone());
            }

            foreach (Turret turret in ship.turrets)
            {
                go.turrets.Add(turret.clone());
            }

            return go;
        }
    }
}
 