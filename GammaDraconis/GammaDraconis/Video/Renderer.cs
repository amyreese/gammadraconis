using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        // The aspect ratio determines how to scale 3d to 2d projection.
        public float aspectRatio;

        // Set the position of the camera in world space, for our view matrix.
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, 1200.0f);

        public GammaDraconis game;

        private Viewport[] viewports;

        private enum Viewports
        {
            WholeWindow = 0,
            TopHalf = 1,
            BottomHalf = 2
        }

        public Renderer(GammaDraconis game) : base(game)
        {
            aspectRatio = (float)game.GraphicsDevice.Viewport.Width /
                          (float)game.GraphicsDevice.Viewport.Height * 2;
            this.game = game;
                      
            viewports = new Viewport[3];
            viewports[(int)Viewports.WholeWindow] = game.GraphicsDevice.Viewport;
            viewports[(int)Viewports.TopHalf] = viewports[(int)Viewports.WholeWindow];
            viewports[(int)Viewports.TopHalf].Height = viewports[(int)Viewports.TopHalf].Height / 2;
            viewports[(int)Viewports.BottomHalf] = viewports[(int)Viewports.TopHalf];
            viewports[(int)Viewports.BottomHalf].Y = viewports[(int)Viewports.BottomHalf].Height;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// Render a frame of video containing the given Scene and Interface.
        /// The Scene manager includes the world, and all contained models.
        /// The interface is drawn last to show a Menu or HUD.
        /// </summary>
        /// <param name="scene">The scene manager</param>
        /// <param name="iface">The menu or HUD interface</param>
        public void render(GameTime gameTime, Scene scene, Interface iface)
        {
            Viewport wholeScreenViewport = new Viewport();
            wholeScreenViewport.X = 0;
            wholeScreenViewport.Y = 0;
            wholeScreenViewport.Width = game.Window.ClientBounds.Width;
            wholeScreenViewport.Height = game.Window.ClientBounds.Height;
            Viewport playerViewport = new Viewport();
            playerViewport.X = 0;
            playerViewport.Y = 0;
            playerViewport.Width = game.Window.ClientBounds.Width;
            playerViewport.Height = game.Window.ClientBounds.Height;
            
            game.GraphicsDevice.Viewport = wholeScreenViewport;
            game.GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.Black, 1.0f, 0);
            game.GraphicsDevice.RenderState.DepthBufferEnable = true;

            int maxPlayerNumber = 0;
            for (int playerIndex = 0; playerIndex < Player.players.Length; playerIndex++)
            {
                if (Player.players[playerIndex] != null)
                {
                    maxPlayerNumber = playerIndex + 1;
                }
            }
            Console.WriteLine(maxPlayerNumber);
            if (maxPlayerNumber == 2)
            {
                playerViewport.Width /= 2;
            }
            else if (maxPlayerNumber == 3 || maxPlayerNumber == 4)
            {
                playerViewport.Width /= 2;
                playerViewport.Height /= 2;
                Console.WriteLine(playerViewport.Height);
            }
            for (int playerIndex = 0; playerIndex < Player.players.Length; playerIndex++)
            {
                if (playerIndex % 2 == 0)
                {
                    playerViewport.X = 0;
                }
                else
                {
                    playerViewport.X = playerViewport.Width;
                }

                if (playerIndex == 2)
                {
                    playerViewport.Y = playerViewport.Height;
                }
                game.GraphicsDevice.Viewport = playerViewport;

                if (Player.players[playerIndex] != null)
                {
                    renderObjects(scene.visible(Player.players[playerIndex].position), Player.players[playerIndex].getCameraLookAtMatrix());
                }
                else
                {
                    game.GraphicsDevice.Clear(Color.Orange);
                }
            }
            if( Player.players.Length == 3 )
            {
                playerViewport.X = playerViewport.Width;
            }
            

            // TODO: Give interfaces for each player, and scale appropriately
            #region Interface rendering
            iface.Draw(gameTime, Vector2.Zero, Vector2.One, 0.0f);
            #endregion

            game.GraphicsDevice.Viewport = wholeScreenViewport;
        }
        
        #region GameObject rendering
        private void renderObjects(List<GameObject> objects, Matrix cameraMatrix )
        {
            Matrix worldMatrix = Matrix.Identity;
            Matrix objectMatrix, subObjectMatrix, modelMatrix;

            foreach (GameObject gameObject in objects)
            {
                objectMatrix = worldMatrix * gameObject.position.matrix();

                foreach (FBXModel fbxmodel in gameObject.models)
                {
                    modelMatrix =  Matrix.CreateScale(fbxmodel.scale) * objectMatrix * fbxmodel.offset.matrix();

                    Model model = fbxmodel.model;
                    if (model == null)
                    {
                        fbxmodel.model = model = game.Content.Load<Model>(fbxmodel.filename);
                    }

                    // Copy any parent transforms.
                    Matrix[] transforms = new Matrix[model.Bones.Count];
                    model.CopyAbsoluteBoneTransformsTo(transforms);

                    // Draw the model. A model can have multiple meshes, so loop.
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        // This is where the mesh orientation is set, as well as our camera and projection.
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.World = transforms[mesh.ParentBone.Index] * modelMatrix;
                            //effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                            effect.View = cameraMatrix;
                            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60.0f),
                                aspectRatio, 1.0f, 90000.0f);
                        }
                        // Draw the mesh, using the effects set above.
                        mesh.Draw();
                    }
                }
            }
        }
        #endregion
    }
}