using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Types;
using GammaDraconis.Video;
using GammaDraconis.Video.GUI;
using GammaDraconis.Screens;

namespace GammaDraconis.Core
{
    /// <summary>
    /// The game engine handles managing most game-related activities 
    /// and delegating actions to other pieces of the game.
    /// </summary>
    class Engine
    {
        private bool AITest = false;

        #region Engine States
        private bool enginePaused = false;
        public static GameTime gameTime;
        #endregion

        #region Constructor
        private static Engine instance;
        public static Engine GetInstance()
        {
            return instance;
        }

        GammaDraconis game;

        /// <summary>
        /// Starts up a game engine for the specified map
        /// </summary>
        /// <param name="mapName">The name given to the map in the file system</param>
        public Engine(GammaDraconis game, String mapName, List<Player> players)
        {
            if (instance != null)
            {
                #region Cleanup the old instance
                instance.Cleanup();
                instance = null;
                #endregion
            }
            instance = this;

            this.game = game;

            // Initialize the Renderer
            SetupGameRenderer();

            //Setup a course
            this.players = players.ToArray();

            SetupCourse(mapName);
        }
        #endregion

        #region Rendering
        private Renderer gameRenderer;
        public Scene gameScene;

        /// <summary>
        /// Initializes the renderer, sets the renderer to focus on Helix, and tells
        /// it where the bounds of the map are so the camera doesn't move too far
        /// </summary>
        private void SetupGameRenderer()
        {
            gameRenderer = GammaDraconis.renderer;
        }

        /// <summary>
        /// Prepare the game to be rendered, and handle the Renderer
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Render(GameTime gameTime)
        {
            gameRenderer.render(gameTime, gameScene);
        }
        #endregion

        private double secondsToEnd = -1;
        private double secondsToStart = Properties.Settings.Default.RaceStartDelay;

        #region Physics/AI/etc
        /// <summary>
        /// Update the current game status, performing AI, collision, and physics processing.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Update(GameTime gameTime)
        {
            Engine.gameTime = gameTime;

            race.update();
            if (secondsToEnd == -1)
            {
                if (race.isRaceOver())
                {
                    secondsToEnd = 2;
                }
            }
            else
            {
                secondsToEnd -= gameTime.ElapsedRealTime.TotalSeconds;
                if (secondsToEnd < 0)
                {
                    ((LevelOverScreen)game.getScreen(GammaDraconis.GameStates.LevelOver)).LevelOver(race);
                    game.changeState(GammaDraconis.GameStates.LevelOver);
                }
            }

            if (secondsToStart > 0)
            {
                secondsToStart -= gameTime.ElapsedRealTime.TotalSeconds;

                foreach (Player player in Player.players)
                {
                    if (player != null)
                    {
                        player.playerHUD.Update(gameTime);
                    }
                }
            }
            else
            {
                Think(gameTime);
                Physics(gameTime);
            }
        }

        #region AI
        /// <summary>
        /// Handles input and initiates the AI for all the characters
        /// </summary>
        /// <param name="gameTime">The game time for this update</param>
        public void Think(GameTime gameTime)
        {
            List<GameObject> gameObjects = gameScene.thinkable();
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.think(gameTime);

                if (AITest)
                {
                    //Make the object follow the course
                    if (gameObject is Racer && !(gameObject is Player))
                    {
                        //Console.WriteLine(gameObject.position.pos());
                        gameObject.throttle(1.0f);

                        Vector3 r = gameObject.position.pos();
                        Vector3 cp = course.path[0].pos();
                        Vector3 rprojection = Vector3.Multiply(r, -2);
                        rprojection = Vector3.Add(r, rprojection);

                        r = Vector3.Subtract(rprojection, r);
                        cp = Vector3.Subtract(cp, r);
                        r.Normalize();
                        cp.Normalize();


                        //Find the angle between the Racer and the Checkpoint and put it into a Quaternion Q
                        float d = Vector3.Dot(r, cp);
                        Vector3 axis = Vector3.Cross(r, cp);
                        float qw = (float)Math.Sqrt(r.LengthSquared() * cp.LengthSquared()) + d;
                        Quaternion q;
                        if (qw < 0.0001)
                        {
                            q = new Quaternion(r.X, r.Y, -r.Z, 0);
                        }
                        else
                        {
                            q = new Quaternion(axis.X, axis.Y, axis.Z, qw);
                        }
                        q.Normalize();

                        //Convert the Quaternion q into Euler angles for Yaw, Pitch, and Roll
                        float yaw = (float)Math.Atan(2 * (q.X * q.Y + q.W * q.Z) / (q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z));
                        float pitch = (float)Math.Asin(-2 * (q.X * q.Z - q.W * q.Y));
                        float roll = (float)Math.Atan(2 * (q.W * q.X + q.Y * q.Z) / (q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z));


                        //Change the rotations of the gameobject
                        gameObject.yaw(yaw);
                        gameObject.pitch(pitch);
                        gameObject.roll(roll);


                        //gameObject.acceleration.R = course.path[0].R;
                        //Vector3 temp = Vector3.Subtract(course.path[0].pos(), gameObject.position.pos());
                        //temp.Normalize();
                        //Quaternion q = Quaternion.CreateFromYawPitchRoll(temp.Y, temp.X, temp.Z);
                        //Quaternion q1 = gameObject.position.R;
                        //q = Quaternion.Lerp(q,q1,4.0f);
                        //gameObject.yaw(q.Y * q.W);
                        //gameObject.pitch(q.X * q.W);
                        //gameObject.roll(q.Z * q.W);

                        //TODO: When the ship reaches a point, make it smoothly go for the next point
                        if (Vector3.Distance(course.path[0].pos(), gameObject.position.pos()) < 10)
                        {
                            Coords temp1 = course.path[0];
                            course.path.RemoveAt(0);
                            course.path.Add(temp1);
                        }
                    }
                    else if (gameObject == Player.players[1])
                    {
                        //gameObject.throttle(0.1f);
                        Vector3 racerPosition = gameObject.position.pos();
                        Vector3 checkpointPosition = race.nextCoord((Racer)gameObject).pos();
                        Vector3 checkpointOffset = checkpointPosition - racerPosition;
                        Vector3 racerForward = Matrix.CreateFromQuaternion(gameObject.position.R).Forward;

                        Vector3 axis = Vector3.Cross(racerForward, checkpointOffset);
                        racerForward.Normalize();
                        checkpointOffset.Normalize();
                        float angle = (float)Math.Acos(Vector3.Dot(racerForward, checkpointOffset));
                        if (float.IsNaN(angle))
                        {
                            angle = 0;
                        }
                        Console.WriteLine(angle);


                        axis.Normalize();

                        Quaternion q = Quaternion.CreateFromAxisAngle(axis, angle);
                        q.Normalize();

                        //Convert the Quaternion q into Euler angles for Yaw, Pitch, and Roll
                        float yaw = (float)Math.Atan(2 * (q.X * q.Y + q.W * q.Z) / (q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z));
                        float pitch = (float)Math.Asin(-2 * (q.X * q.Z - q.W * q.Y));
                        float roll = (float)Math.Atan(2 * (q.W * q.X + q.Y * q.Z) / (q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z));

                        if (float.IsNaN(yaw))
                        {
                            Console.WriteLine("!!!");
                        }
                        if (float.IsNaN(pitch))
                        {
                            Console.WriteLine("!!!");
                        }
                        if (float.IsNaN(roll))
                        {
                            Console.WriteLine("!!!");
                        }

                        //Change the rotations of the gameobject
                        gameObject.yaw(yaw);
                        gameObject.pitch(pitch);
                        gameObject.roll(roll);
                    }
                    else if (gameObject == Player.players[2])
                    {
                        //gameObject.throttle(0.1f);
                        Vector3 racerPosition = gameObject.position.pos();
                        Vector3 checkpointPosition = race.nextCoord((Racer)gameObject).pos();
                        Vector3 checkpointOffset = checkpointPosition - racerPosition;
                        Vector3 racerForward = Matrix.CreateFromQuaternion(gameObject.position.R).Forward;

                        Vector3 axis = Vector3.Cross(checkpointOffset, racerForward);
                        racerForward.Normalize();
                        checkpointOffset.Normalize();
                        float angle = (float)Math.Acos(Vector3.Dot(racerForward, checkpointOffset));

                        Quaternion q = Quaternion.CreateFromAxisAngle(axis, angle);
                        q.Normalize();
                        q = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAt(racerPosition, checkpointPosition, Vector3.Up));

                        //Convert the Quaternion q into Euler angles for Yaw, Pitch, and Roll
                        float yaw = (float)Math.Atan(2 * (q.X * q.Y + q.W * q.Z) / (q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z));
                        float pitch = (float)Math.Asin(-2 * (q.X * q.Z - q.W * q.Y));
                        float roll = (float)Math.Atan(2 * (q.W * q.X + q.Y * q.Z) / (q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z));

                        if (float.IsNaN(yaw))
                        {
                            Console.WriteLine("!!!");
                        }
                        if (float.IsNaN(pitch))
                        {
                            Console.WriteLine("!!!");
                        }
                        if (float.IsNaN(roll))
                        {
                            Console.WriteLine("!!!");
                        }

                        //Change the rotations of the gameobject
                        /*
                        gameObject.yaw(yaw);
                        gameObject.pitch(pitch);
                        gameObject.roll(roll);
                        /* */

                        gameObject.position.R = Quaternion.Slerp(gameObject.position.R, q, 1.0f);
                    }
                }
            }
        }
        #endregion

        #region Physics
        /// <summary>
        /// Movement and Collision Detection
        /// </summary>
        /// <param name="gameTime">The game time for this update</param>
        public void Physics(GameTime gameTime)
        {
            // If it's paused, don't do anything
            if (enginePaused)
            {
                return;
            }

            float timeMod = (float)gameTime.ElapsedRealTime.TotalSeconds;
            List<GameObject> collidableGameObjects = gameScene.collidable();
            foreach (GameObject o in new List<GameObject>(collidableGameObjects))
            {
                if (o.shieldModel != null)
                {
                    if (o.shieldVisibilityTimer > 0)
                    {
                        o.shieldVisibilityTimer -= gameTime.ElapsedRealTime.TotalSeconds;
                        o.shieldModel.visible = true;
                    }
                    else
                    {
                        o.shieldModel.visible = false;
                    }
                }
                if (o is Bullet)
                {
                    ((Bullet)o).timeToLive -= gameTime.ElapsedRealTime.TotalSeconds;
                    if (((Bullet)o).timeToLive < 0)
                    {
                        gameScene.ignore(o, GO_TYPE.BULLET);
                    }
                }
                if (o.health < 0)
                {
                    o.OnDeath();
                }
                if (o.shield < o.maxShield)
                {
                    o.shield += timeMod * o.shieldIncreaseRate;
                    if (o.shield > o.maxShield)
                    {
                        o.shield = o.maxShield;
                    }
                }
                collidableGameObjects.Remove(o);
                Vector3 oPos = o.position.pos();
                foreach (GameObject o2 in collidableGameObjects)
                {
                    if (o is Bullet && o2 is Bullet)
                    {
                        continue;
                    }
                    if (o.ownedBy == o2 || o2.ownedBy == o)
                    {
                        continue;
                    }
                    if (o.immobile && o2.immobile && o.invincible && o2.invincible)
                    {
                        continue;
                    }
                    if (o is Player && o2 is Player && (o.invincible || o2.invincible))
                    {
                        continue;
                    }
                    Vector3 o2Pos = o2.position.pos();
                    if ((oPos - o2Pos).LengthSquared() <= ((o.size + o2.size) * (o.size + o2.size)))
                    {
                        if (oPos == o2Pos)
                        {
                            // TODO: Randomize some
                            o.position.T *= Matrix.CreateTranslation(new Vector3(0, 0, 0.1f));
                        }
                        float closeness = (o.size + o2.size) - (o.position.pos() - o2.position.pos()).Length();

                        //collision formulas taken from http://www.wheatchex.com/projects/collisions/
                        //Seem to work ok with bullets, but ships colliding not working so well?

                        //restitution coeff
                        float e = 0.75f;

                        //normal unit vector from o to o2.
                        Vector3 n = o2.position.pos() - o.position.pos();
                        n.Normalize();

                        float c = Vector3.Dot(n, (o.velocity.pos() - o2.velocity.pos())) + closeness / 50 + 0.01f;

                        Vector3 mod = (c / (o.mass + o2.mass)) * (1 + e) * n;
                        Vector3 oVelocityMod = o2.mass * mod;
                        Vector3 o2VelocityMod = o.mass * mod;
                        Vector3 oVelocity = o.velocity.pos() - oVelocityMod;
                        Vector3 o2Velocity = o2.velocity.pos() + o2VelocityMod;
                        o.velocity.T = Matrix.CreateTranslation(oVelocity);
                        o2.velocity.T = Matrix.CreateTranslation(o2Velocity);

                        float magnitude = mod.Length() * o.mass * o2.mass / 25;
                        o.takeDamage(magnitude);
                        o2.takeDamage(magnitude);
                        if (o is Bullet)
                        {
                            gameScene.ignore(o, GO_TYPE.BULLET);
                            o2.takeDamage(((Bullet)o).damage);
                        }
                        if (o2 is Bullet)
                        {
                            gameScene.ignore(o2, GO_TYPE.BULLET);
                            o.takeDamage(((Bullet)o2).damage);
                        }
                    }
                }
            }
            /*
             * Physics Handling Pseudocode
             * 
             * Collision Detection
             *   get tree of collidable objects from Scene
             *   foreach object
             *     if collided
             *       add acceleration to each object proportional to each mass
             * 
             * Object Movement
             *   get tree of non-scenery objects from Scene
             *   foreach object
             *     scale down velocity matrix/quat based on mass/drag
             *     apply acceleration matrix/quat to velocity based on positional rotation
             *     apply velocity matrix/quat to position
             *     zero acceleration matrix/quat
             * 
             */

            #region Movement physics
            List<GameObject> gameObjects = gameScene.movable();

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.immobile)
                {
                    continue;
                }
                // Calculate timesteps
                float timestep = gameTime.ElapsedGameTime.Milliseconds / 1000f; // Target 1+ fps

                // Calculate drag
                float dragL = gameObject.dragL;
                float dragR = gameObject.dragR;
                dragL *= timestep;
                dragR *= timestep;

                // Subtract drag from velocity
                gameObject.velocity.R = Quaternion.Slerp(Quaternion.Identity, gameObject.velocity.R, 1 - dragR);
                gameObject.velocity.T = mScale(gameObject.velocity.T, 1 - dragL);

                // Apply acceleration to velocity
                gameObject.velocity.R *= Quaternion.Slerp(Quaternion.Identity, gameObject.acceleration.R, timestep);

                Matrix deltaV = mScale(gameObject.acceleration.T, timestep);
                deltaV = mScale(deltaV, gameObject.rateL);
                deltaV = deltaV * Matrix.CreateFromQuaternion(gameObject.position.R);
                gameObject.velocity.T *= Matrix.CreateTranslation(deltaV.Translation);

                // Chop off velocity at a certain threshold
                Vector3 v = gameObject.velocity.pos();
                bool save = false;
                float cutoff = 0.001f;
                if (Math.Abs(v.X) < cutoff)
                {
                    v.X = 0f;
                    save = true;
                }
                if (Math.Abs(v.Y) < cutoff)
                {
                    v.Y = 0f;
                    save = true;
                }
                if (Math.Abs(v.Z) < cutoff)
                {
                    v.Z = 0f;
                    save = true;
                }
                if (save)
                {
                    gameObject.velocity.T = Matrix.CreateTranslation(v);
                }

                // Apply velocity to position
                Quaternion rotation = Quaternion.Slerp(Quaternion.Identity, gameObject.velocity.R, timestep);
                rotation = qScale(rotation, gameObject.rateV);
                gameObject.position.R *= rotation;

                Matrix deltaP = mScale(gameObject.velocity.T, timestep);
                deltaP = mScale(deltaP, gameObject.rateV);
                gameObject.position.T *= deltaP;

                // Zero acceleration
                gameObject.acceleration.R = Quaternion.Identity;
                gameObject.acceleration.T = Matrix.Identity;
            }
            #endregion
        }
        #endregion
        #endregion

        #region Race & Course
        public Race race;
        public Course course;
        public Player[] players;

        /// <summary>
        /// Set up the course for AI's to follow.  This should be moved later
        /// </summary>
        public void SetupCourse(String mapName)
        {
            GammaDraconis.GetInstance().GameLua.LoadMap(mapName);
            int racePosition = 1;
            foreach (Coords coords in course.path) 
            {
		        GameObject gameObject = Proto.getThing("Checkpoint", coords);
                Checkpoint checkpoint = Checkpoint.cloneObject(gameObject);
                checkpoint.racePosition = racePosition;
                gameScene.track(checkpoint, GO_TYPE.CHECKPOINT);
                racePosition = racePosition + 1;
            }
        }
        #endregion

        /// <summary>
        /// Clean up any memory that wouldn't clean up itself
        /// </summary>
        private void Cleanup()
        {
        }

        /// <summary>
        /// Scale a quaternion by an arbitrary factor.
        /// </summary>
        /// <param name="q">Quaternion to be scaled</param>
        /// <param name="s">The scaling factor</param>
        /// <returns>Scaled quaternion</returns>
        private Quaternion qScale(Quaternion q, float s)
        {
            Quaternion q1 = Quaternion.Identity;

            while (s > 1)
            {
                q1 *= q;
                s -= 1f;
            }

            if (s > 0)
            {
                q1 = Quaternion.Slerp(q1, q1 * q, s);
            }

            return q1;
        }

        /// <summary>
        /// Scale a translation matrix by an arbitrary factor.
        /// </summary>
        /// <param name="m">Translation matrix to be scaled</param>
        /// <param name="s">The scaling factor</param>
        /// <returns>Scaled translation matrix</returns>
        private Matrix mScale(Matrix m, float s)
        {
            Vector3 v = m.Translation;
            v = Vector3.Multiply(v, s);
            return Matrix.CreateTranslation(v);
        }
    }
}
