using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using GammaDraconis.Screens;
using GammaDraconis.Core;
using GammaDraconis.Core.Input;
using GammaDraconis.Video;
using GammaDraconis.Video.GUI;

namespace GammaDraconis
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GammaDraconis : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;

        public bool GameStarted = false;

        // One renderer to rule them all
        internal static Renderer renderer;

        public GammaDraconis()
        {
            graphics = new GraphicsDeviceManager(this);

            Audio.init();
            Input.reset();

            renderer = new Renderer(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameLua = new GameLua();
            Proto.init();

#if DEBUG
            DebugInterface = (Interface)GameLua.DoString("return dofile( 'Interfaces/DebugInterface/DebugInterface.lua' )")[0];
#endif
            initializeGameScreens();
            IsFixedTimeStep = false;
            Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            renderer.reset();

            base.Initialize();
        }

        private GameLua _GameLua;
        internal GameLua GameLua
        {
            get
            {
                return _GameLua;
            }
            private set
            {
                _GameLua = value;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        public void ToggleFullscreen()
        {
            graphics.ToggleFullScreen();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
#if DEBUG
            if (debugFramerate)
            {
                frames = frames + 1;
                double fps = 0;
                fps = Math.Round(1000 / gameTime.ElapsedRealTime.TotalMilliseconds);
                framerates[currentFramerateIndex++] = fps;
                if (currentFramerateIndex >= framerates.Length)
                {
                    currentFramerateIndex = 0;
                }
                double avgFps = 0;
                for (int index = 0; index < framerates.Length; index++)
                {
                    avgFps += framerates[index];
                }
                avgFps /= framerates.Length;
                lastAverageFramerate = avgFps;
                debug("Average FPS: " + avgFps);
                debug("FPS: " + fps);
                if (gameTime.IsRunningSlowly)
                {
                    debug("Slow!!!");
                }
            }
#endif
            base.Draw(gameTime);
        }

        #region Debug Flags, Variables, and Helpers
#if DEBUG
        // Flags
        public static bool debugFramerate = false;
        public static bool debugCameraPosition = false;
        public static bool debugHelixPosition = false;
        public static bool debugCulling = false;
        public static bool debugEffectAndTextureLoading = false;
        public static bool debugCollisions = false;
        public static bool debugBoundingBoxes = false;
        public static bool debugFlying = false;
        public static bool debugTriggers = false;

        // Framerate variables
        private int frames = 0;
        private int currentFramerateIndex = 0;
        private double lastAverageFramerate = 0;
        private double[] framerates = new double[50];

        // Debug Overlay
        public Interface DebugInterface;
#endif
        /// <summary>
        /// Writes out a debug statement, if we're in debug mode
        /// </summary>
        /// <param name="msg"></param>
        [Conditional("DEBUG")]
        public static void debug(string msg)
        {
            Debug.WriteLine(msg);
        }
        #endregion

        private static GammaDraconis _instance;
        public static GammaDraconis GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GammaDraconis();
            }
            return _instance;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            GetInstance().Run();
        }

        #region Game States and Screens

        public enum GameStates
        {
            MainMenu,
            LevelSelectMenu,
            KeyBindingsMenu,
            GameLoading,
            Game,
            LevelOver,
            HighScoreListMenu,
            HighScores,
            CheatMenu
        }

        private GameStates currentGameState = GameStates.MainMenu;

        public void changeState(GameStates toState)
        {
            if (!screens.ContainsKey(toState))
            {
                throw new Exception("Attempt to change to a state without a screen.");
            }
            screens[currentGameState].Enabled = false;
            screens[toState].Enabled = true;
            screens[currentGameState].Visible = false;
            screens[toState].Visible = true;
            currentGameState = toState;
        }

        private Dictionary<GameStates, Screen> screens;
        internal Screen getScreen(GameStates forState)
        {
            return screens[forState];
        }

        protected void initializeGameScreens()
        {
            /* TODO: use this for levels like in Snails Pace?
            String levelSplit = gameConfig.getString("levelSplit");
            String levelSubsplit = gameConfig.getString("levelSubsplit");
            String[] levels = gameConfig.getString("levels").Split(levelSplit.ToCharArray());
            String[][] levelsSetting = new String[levels.Length][];
            for (int index = 0; index < levels.Length; index++)
            {
                levelsSetting[index] = levels[index].Split(levelSubsplit.ToCharArray());
            }
             * */
            screens = new Dictionary<GameStates, Screen>();
            screens.Add(GameStates.MainMenu, new Screens.Menus.MainMenu(this));
            screens.Add(GameStates.GameLoading, new Screens.LevelLoadingScreen(this));
            screens.Add(GameStates.Game, new Screens.GameScreen(this));
            screens.Add(GameStates.LevelOver, new Screens.LevelOverScreen(this));
            /*
            screens.Add(GameStates.LevelSelectMenu, new Screens.Menus.LevelSelectScreen(this, levelsSetting));
            screens.Add(GameStates.KeyBindingsMenu, new Screens.Menus.KeyBindingsMenuScreen(this));
            screens.Add(GameStates.HighScoreListMenu, new Screens.Menus.HighScoreMenuScreen(this));
            screens.Add(GameStates.HighScores, new Screens.HighScoreScreen(this, "Test"));
            screens.Add(GameStates.CheatMenu, new Screens.Menus.CheatMenuScreen(this));
             */

            Dictionary<GameStates, Screen>.Enumerator screenEnumerator = screens.GetEnumerator();
            while (screenEnumerator.MoveNext())
            {
                Components.Add(screenEnumerator.Current.Value);
                screenEnumerator.Current.Value.Enabled = screenEnumerator.Current.Key == currentGameState;
                screenEnumerator.Current.Value.Visible = screenEnumerator.Current.Value.Enabled;
            }
        }

        #endregion

    }
}
