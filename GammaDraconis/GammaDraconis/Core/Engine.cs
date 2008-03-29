using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Types;
using GammaDraconis.Video;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Core
{
    /// <summary>
    /// The game engine handles managing most game-related activities 
    /// and delegating actions to other pieces of the game.
    /// </summary>
    class Engine
    {
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
        public Engine(GammaDraconis game, String mapName)
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
            SetupCourse( mapName );
        }
        #endregion

        #region Rendering
        private Renderer gameRenderer;
        private Scene gameScene;
        
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

        #region Physics/AI/etc
        /// <summary>
        /// Update the current game status, performing AI, collision, and physics processing.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Update( GameTime gameTime )
        {
            Engine.gameTime = gameTime;
 
            Think(gameTime);
            Physics(gameTime);
        }

        #region AI

        private Course course;

        /// <summary>
        /// Set up the course for AI's to follow.  This should be moved later
        /// </summary>
        public void SetupCourse(String mapName)
        {
            gameScene = new Scene();
            course = new Course();

            GammaDraconis.GetInstance().GameLua.LoadMap(mapName);

            for (int pathIndex = 0; pathIndex < 5; pathIndex++)
            {
                course.path.Add(new Coords(20.0f * pathIndex, 12.0f * pathIndex, -28.0f * pathIndex));
                //TODO: Make each coordinate point at the next coordinate
            }
            
        }

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

                //Make the object follow the course
                if (gameObject is Racer && !(gameObject is Player))
                {
                    
                    gameObject.throttle(1.0f);

                    //Find the angle between the Racer and the Checkpoint and put it into a Quaternion Q
                    float d = Vector3.Dot(gameObject.position.pos(), course.path[0].pos());
                    Vector3 axis = Vector3.Cross(gameObject.position.pos(), course.path[0].pos());
                    float qw = (float)Math.Sqrt(gameObject.position.pos().LengthSquared() * course.path[0].pos().LengthSquared()) + d;
                    Quaternion q;
                    if (qw < 0.0001) { q = new Quaternion(gameObject.position.pos().X, gameObject.position.pos().Y, -gameObject.position.pos().Z, 0); }
                    else { q = new Quaternion(axis.X, axis.Y, axis.Z, qw); }
                    q.Normalize();

                    //Convert the Quaternion q into Euler angles for Yaw, Pitch, and Roll
                    double yaw = Math.Atan(2 * (q.X * q.Y + q.W * q.Z) / (q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z));
                    double pitch = Math.Asin(-2 * (q.X * q.Z - q.W * q.Y));
                    double roll = Math.Atan(2 * (q.W * q.X + q.Y * q.Z) / (q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z));

                    //Change the rotations of the gameobject
                    gameObject.yaw((float)yaw);
                    gameObject.pitch((float)pitch);
                    gameObject.roll((float)roll);

                    

                    /*gameObject.acceleration.R = course.path[0].R;
                    Vector3 temp = Vector3.Subtract(course.path[0].pos(), gameObject.position.pos());
                    temp.Normalize();
                    Quaternion q = Quaternion.CreateFromYawPitchRoll(temp.Y, temp.X, temp.Z);
                    Quaternion q1 = gameObject.position.R;
                    q = Quaternion.Lerp(q,q1,4.0f);
                    gameObject.yaw(q.Y * q.W);
                    gameObject.pitch(q.X * q.W);
                    gameObject.roll(q.Z * q.W);*/

                    //TODO: When the ship reaches a point, make it smoothly go for the next point
                    if (Vector3.Distance(course.path[0].pos(), gameObject.position.pos()) < 10)
                    {
                        Coords temp1 = course.path[0];
                        course.path.RemoveAt(0);
                        course.path.Add(temp1);
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
                deltaV = deltaV * Matrix.CreateFromQuaternion(gameObject.position.R);
                gameObject.velocity.T *= Matrix.CreateTranslation(deltaV.Translation);

                // Apply velocity to position
                Quaternion rotation = Quaternion.Slerp(Quaternion.Identity, gameObject.velocity.R, timestep);
                rotation = qScale(rotation, gameObject.rateR);
                gameObject.position.R *= rotation;

                Matrix deltaP = mScale(gameObject.velocity.T, timestep);
                deltaP = mScale(deltaP, gameObject.rateL);
                gameObject.position.T *= deltaP;

                // Zero acceleration
                gameObject.acceleration.R = Quaternion.Identity;
                gameObject.acceleration.T = Matrix.Identity;
            }
            #endregion
        }
        #endregion
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
