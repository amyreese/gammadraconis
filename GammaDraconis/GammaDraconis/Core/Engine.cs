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
        }
        #endregion

        #region Rendering
        private Renderer gameRenderer;
        private Scene gameScene;
        private Interface gameInterface;

        /// <summary>
        /// Initializes the renderer, sets the renderer to focus on Helix, and tells
        /// it where the bounds of the map are so the camera doesn't move too far
        /// </summary>
        private void SetupGameRenderer()
        {
            gameRenderer = new Renderer(game);
            gameScene = new Scene();

            Player p = new Player(PlayerIndex.One);
            Racer r = new Racer();
            r.position = new Coords(200.0f, -1200.0f, -2800.0f);
            r.models[0].scale = 2f;
            gameScene.track(p, GO_TYPE.RACER);
            gameScene.track(r, GO_TYPE.RACER);

            GameObject planet = new GameObject();
            planet.position = new Coords(0f, 0f, -50000f);
            planet.models.Add(new FBXModel("Resources/Models/Planet", "", 50f));
            gameScene.track(planet, GO_TYPE.SCENERY);

            gameInterface = new Interface(game);
            gameInterface.Enabled = true;
            gameInterface.Visible = true;
            gameInterface.RelativePosition = new Vector2(50f, 50f);
        }

        /// <summary>
        /// Prepare the game to be rendered, and handle the Renderer
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Render(GameTime gameTime)
        {
            gameRenderer.render(gameTime, gameScene, gameInterface);
        }
        #endregion

        #region Physics/AI/etc
        /// <summary>
        /// Update the current game status, performing AI, collision, and physics processing.
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Update( GameTime gameTime )
        {
            Think(gameTime);
            Physics(gameTime);
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
                // TODO: Implement drag
                //gameObject.velocity.T *= Matrix.CreateScale(1 - (100 * gameObject.drag / gameObject.mass));
                //gameObject.velocity.R.W = gameObject.velocity.R.W * .5f;

                // TODO: Apply acceleration to velocity
                gameObject.velocity.R *= gameObject.acceleration.R;
                gameObject.velocity.T *= gameObject.acceleration.T;

                // TODO: Clamp velocity to velocityMax
                
                // TODO: Apply velocity to position
                gameObject.position.R *= gameObject.velocity.R;
                gameObject.position.T *= gameObject.velocity.T;

                // TODO: Zero acceleration
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
    }
}
