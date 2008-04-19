using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Core;
using GammaDraconis.Types;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Video
{
    /// <summary>
    /// The renderer handles converting a set of models and sprites 
    /// into screen graphics all fancy-like.
    /// </summary>
    class Renderer : DrawableGameComponent
    {
        private int secondsPerQuip = 5;
        private String[] missingPlayerQuips = { 
            "No Player!", 
            "Ningun Jugador!", 
            "Kein Spieler!", 
            "Nenhum Jogador!",
            "Nessun Giocatore!",
            "Geen Speler!",
            "Aucun Joueur!"
        };

        // The aspect ratio determines how to scale 3d to 2d projection.
        public float aspectRatio;
        public float viewingAngle;
        public float viewingDistance;

        public GammaDraconis game;

        private Viewport[] viewports;

        private Viewports[] MissingPlayerViewports;

        // Lighting properties
        public Effect baseEffect;
        public PointLight[] lights;

        // Post process shaders
        public Dictionary<string, PostProcessShader> shaders;

        // Bloom shader
        public BloomComponent bloom;
        public MotionBlurComponent blur;

        public enum Viewports
        {
            None = -1,
            WholeWindow = 0,
            TopLeft = 1,
            TopRight = 2,
            BottomLeft = 3,
            BottomRight = 4,
            TopHalf = 5,
            BottomHalf = 6,
            LeftSide = 7,
            RightSide = 8,
        }

        public Renderer(GammaDraconis game)
            : base(game)
        {
            aspectRatio = 0;
            viewingAngle = 60f;
            viewingDistance = 15000f;

            this.game = game;
            game.Window.ClientSizeChanged += new EventHandler(Window_ClientSizeChanged);
            viewports = new Viewport[9];

            PostProcessShader shader = new PostProcessShader(game);
            shader.effects.Add("Resources/Effects/BloomExtract");
            shader.divisions.Add(1);
            /*
            shader.effects.Add("Resources/Effects/BloomExtract");
            shader.divisions.Add(2);
            shader.effects.Add("Resources/Effects/GaussianBlur");
            shader.divisions.Add(2);
            shader.effects.Add("Resources/Effects/GaussianBlur");
            shader.divisions.Add(2);
            shader.effects.Add("Resources/Effects/BloomCombine");
            shader.divisions.Add(2);
             */


            shaders = new Dictionary<string, PostProcessShader>();
            shaders.Add("bloom", shader);

            bloom = new BloomComponent(game);
            blur = new MotionBlurComponent(game);

            reset();
        }

        public void reset()
        {
            InitializeViewports();
        }

        private void InitializeViewports()
        {
            viewports[(int)Viewports.WholeWindow] = new Viewport();
            viewports[(int)Viewports.WholeWindow].X = 0;
            viewports[(int)Viewports.WholeWindow].Y = 0;

            viewports[(int)Viewports.WholeWindow].Width = game.Window.ClientBounds.Width;
            viewports[(int)Viewports.WholeWindow].Height = game.Window.ClientBounds.Height;

            viewports[(int)Viewports.TopHalf] = viewports[(int)Viewports.WholeWindow];
            viewports[(int)Viewports.TopHalf].Height = viewports[(int)Viewports.TopHalf].Height / 2;

            viewports[(int)Viewports.BottomHalf] = viewports[(int)Viewports.TopHalf];
            viewports[(int)Viewports.BottomHalf].Y = viewports[(int)Viewports.BottomHalf].Height;

            viewports[(int)Viewports.TopLeft] = viewports[(int)Viewports.TopHalf];
            viewports[(int)Viewports.TopLeft].Width = viewports[(int)Viewports.TopLeft].Width / 2;

            viewports[(int)Viewports.TopRight] = viewports[(int)Viewports.TopLeft];
            viewports[(int)Viewports.TopRight].X = viewports[(int)Viewports.TopLeft].X + (game.Window.ClientBounds.Width / 2);

            viewports[(int)Viewports.BottomLeft] = viewports[(int)Viewports.BottomHalf];
            viewports[(int)Viewports.BottomLeft].Width = viewports[(int)Viewports.BottomLeft].Width / 2;

            viewports[(int)Viewports.BottomRight] = viewports[(int)Viewports.BottomLeft];
            viewports[(int)Viewports.BottomRight].X = viewports[(int)Viewports.BottomRight].X + (game.Window.ClientBounds.Width / 2);

            viewports[(int)Viewports.LeftSide] = viewports[(int)Viewports.TopLeft];
            viewports[(int)Viewports.LeftSide].Height = viewports[(int)Viewports.LeftSide].Height * 2;

            viewports[(int)Viewports.RightSide] = viewports[(int)Viewports.TopRight];
            viewports[(int)Viewports.RightSide].Height = viewports[(int)Viewports.RightSide].Height * 2;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            InitializeViewports();
        }

        /// <summary>
        /// Render a frame of video containing the given Scene and Interface.
        /// The Scene manager includes the world, and all contained models.
        /// The interface is drawn last to show a Menu or HUD.
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="scene">The scene manager</param>
        public void render(GameTime gameTime, Scene scene) { render(gameTime, scene, true); }
        public void render(GameTime gameTime, Scene scene, bool drawHUD)
        {
            int numPlayers = SetPlayerViewports();

            PostProcessShader shader = shaders["bloom"];
            shader.reset();

            // Render all players' viewports
            for (int playerIndex = 0; playerIndex < Player.players.Length; playerIndex++)
            {
                if (Player.players[playerIndex] != null)
                {
                    game.GraphicsDevice.Viewport = viewports[(int)Player.players[playerIndex].viewport];
                    game.GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.Black, 1.0f, 0);
                    game.GraphicsDevice.RenderState.DepthBufferEnable = true;
                    aspectRatio = game.GraphicsDevice.Viewport.AspectRatio;

                    List<GameObject> gameObjects = scene.visible(Player.players[playerIndex].getCamera());
                    renderObjects(gameObjects, Player.players[playerIndex].getCameraLookAtMatrix());

                    // Uncomment this line for motion blur!
                    //blur.Render((Player.players[playerIndex].velocity.pos().Length() / Player.players[playerIndex].maxVelocity) * 0.5f);
                }
                else
                {
                    if (MissingPlayerViewports[playerIndex] != Viewports.None)
                    {
                        game.GraphicsDevice.Viewport = viewports[(int)MissingPlayerViewports[playerIndex]];
                        game.GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.Black, 1.0f, 0);
                        Interface i = new Interface(game);
                        Text t = new Text(game);
                        t.spriteFontName = "Resources/Fonts/Menu";
                        t.text = missingPlayerQuips[(gameTime.TotalRealTime.Seconds / secondsPerQuip) % missingPlayerQuips.Length];
                        t.center = true;
                        t.RelativePosition = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);
                        t.color = Color.WhiteSmoke;
                        i.AddComponent(t);
                        i.Draw(gameTime, Vector2.Zero, Vector2.One, 0);
                        // TODO: Draw something noteworthy in the empty slots?
                    }
                }
            }

            game.GraphicsDevice.Viewport = viewports[(int)Viewports.WholeWindow];
            shader.Render();

            // Bloom post-processing
            //bloom.Render();

            // Render players' HUDs
            if (drawHUD)
            {
                for (int playerIndex = 0; playerIndex < Player.players.Length; playerIndex++)
                {
                    if (Player.players[playerIndex] != null)
                    {
                        game.GraphicsDevice.Viewport = viewports[(int)Player.players[playerIndex].viewport];

                        Vector2 scale = new Vector2(game.GraphicsDevice.Viewport.Width / 1024.0f, game.GraphicsDevice.Viewport.Height / 768.0f);
                        Player.players[playerIndex].playerHUD.Draw(gameTime, Vector2.Zero, scale, 0);
                    }
                }
            }

            // Reset viewports
            game.GraphicsDevice.Viewport = viewports[(int)Viewports.WholeWindow];
        }

        /// <summary>
        /// Render a scene in the entire window from an arbirtrary vantage point.
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="scene">The scene manager</param>
        /// <param name="coords">The position to view the scene from</param>
        public void render(GameTime gameTime, Scene scene, Coords coords)
        {
            game.GraphicsDevice.Viewport = viewports[(int)Viewports.WholeWindow];
            game.GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.Black, 1.0f, 0);
            game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            aspectRatio = game.GraphicsDevice.Viewport.AspectRatio;

            PostProcessShader shader = shaders["bloom"];
            shader.reset();

            List<GameObject> gameObjects = scene.visible(coords);
            renderObjects(gameObjects, coords.camera());

            shader.Render();

            //bloom.Render();
        }

        /// <summary>
        /// Render a set of objects with a given camera matrix.
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="cameraMatrix"></param>
        private void renderObjects(List<GameObject> objects, Matrix cameraMatrix)
        {
            Matrix worldMatrix = Matrix.Identity;
            Matrix objectMatrix, modelMatrix;

            PostProcessShader shader = shaders["bloom"];

            foreach (GameObject gameObject in objects)
            {
                objectMatrix = worldMatrix * gameObject.position.matrix();

                List<FBXModel> fbxmodels = new List<FBXModel>(gameObject.models);
                fbxmodels.Add(gameObject.shieldModel);

                foreach (FBXModel fbxmodel in fbxmodels)
                {
                    if (fbxmodel == null || !fbxmodel.visible)
                    {
                        continue;
                    }

                    modelMatrix = Matrix.CreateScale(fbxmodel.scale) * objectMatrix * fbxmodel.offset.matrix();

                    Model model = fbxmodel.model;

                    // Copy any parent transforms.
                    Matrix[] transforms = new Matrix[model.Bones.Count];
                    model.CopyAbsoluteBoneTransformsTo(transforms);

                    // Draw the model. A model can have multiple meshes, so loop.
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        // This is where the mesh orientation is set, as well as our camera and projection.
                        foreach (BasicEffect mesheffect in mesh.Effects)
                        {
                            mesheffect.PreferPerPixelLighting = Properties.Settings.Default.PerPixelLighting;
                            mesheffect.EnableDefaultLighting();
                            mesheffect.World = transforms[mesh.ParentBone.Index] * modelMatrix;
                            //effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                            mesheffect.View = cameraMatrix;
                            mesheffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(viewingAngle),
                                GammaDraconis.GetInstance().GraphicsDevice.Viewport.AspectRatio, 0.1f, viewingDistance);
                        }

                        // Draw the mesh, using the effects set above.
                        game.GraphicsDevice.SetRenderTarget(0, null);
                        mesh.Draw();

                        // Draw the mesh to the shader source
                        game.GraphicsDevice.SetRenderTarget(1, shader.source);
                        mesh.Draw();

                        game.GraphicsDevice.SetRenderTarget(1, null);

                    }
                }
            }

        }

        private int SetPlayerViewports()
        {
            MissingPlayerViewports = new Viewports[Player.players.Length];
            for( int x = 0; x < MissingPlayerViewports.Length; x++ ) {
                MissingPlayerViewports[x] = Viewports.None;
            }
            int numPlayers = 0;
            for (int playerIndex = 0; playerIndex < Player.players.Length; playerIndex++)
            {
                if (Player.players[playerIndex] != null)
                {
                    numPlayers++;
                }
            }

            if (numPlayers == 1)
            {
                for (int playerIndex = 0; playerIndex < Player.players.Length; playerIndex++)
                {
                    if (Player.players[playerIndex] != null)
                    {
                        Player.players[playerIndex].viewport = Viewports.WholeWindow;
                    }
                }
            }
            else if (numPlayers == 2)
            {
                bool foundFirst = false;
                for (int playerIndex = 0; playerIndex < Player.players.Length; playerIndex++)
                {
                    if (Player.players[playerIndex] != null)
                    {
                        if (!foundFirst)
                        {
                            foundFirst = true;
                            Player.players[playerIndex].viewport = Viewports.TopHalf;
                        }
                        else
                        {
                            Player.players[playerIndex].viewport = Viewports.BottomHalf;
                        }
                    }
                }
            }
            else if (numPlayers == 3 || numPlayers == 4)
            {
                if (Player.players[0] != null)
                {
                    Player.players[0].viewport = Viewports.TopLeft;
                }
                else
                {
                    MissingPlayerViewports[0] = Viewports.TopLeft;
                }
                if (Player.players[1] != null)
                {
                    Player.players[1].viewport = Viewports.TopRight;
                }
                else
                {
                    MissingPlayerViewports[1] = Viewports.TopRight;
                }
                if (Player.players[2] != null)
                {
                    Player.players[2].viewport = Viewports.BottomLeft;
                }
                else
                {
                    MissingPlayerViewports[2] = Viewports.BottomLeft;
                }
                if (Player.players[3] != null)
                {
                    Player.players[3].viewport = Viewports.BottomRight;
                }
                else
                {
                    MissingPlayerViewports[3] = Viewports.BottomRight;
                }
            }

            return numPlayers;
        }

        protected override void LoadContent()
        {
            if (game.GraphicsDevice.GraphicsDeviceCapabilities.PixelShaderVersion.Major >= 3)
            {
                baseEffect = game.Content.Load<Effect>("Resources\\Effects\\MaterialShader30");
                lights = new PointLight[8];
            }
            else
            {
                baseEffect = game.Content.Load<Effect>("Resources\\Effects\\MaterialShader20");
                lights = new PointLight[2];
            }

            base.LoadContent();
        }
    }
}