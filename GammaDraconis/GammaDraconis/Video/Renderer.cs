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
    class Renderer
    {
        // The aspect ratio determines how to scale 3d to 2d projection.
        public float aspectRatio;

        // Set the position of the camera in world space, for our view matrix.
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, 1200.0f);

        public GammaDraconis game;

        public Renderer(GammaDraconis game)
        {
            aspectRatio = (float)game.GraphicsDevice.Viewport.Width /
                          (float)game.GraphicsDevice.Viewport.Height;
            this.game = game;
        }

        /// <summary>
        /// Render a frame of video containing the given Scene and Interface.
        /// The Scene manager includes the world, and all contained models.
        /// The interface is drawn last to show a Menu or HUD.
        /// </summary>
        /// <param name="scene">The scene manager</param>
        /// <param name="iface">The menu or HUD interface</param>
        public void render(Scene scene, Interface iface)
        {
            List<GameObject> objects = scene.visible(Player.players[0].position);

            game.GraphicsDevice.Clear(Color.Black);
            game.GraphicsDevice.RenderState.DepthBufferEnable = true;

            #region GameObject rendering
            Matrix worldMatrix = Matrix.Identity;
            Matrix objectMatrix, subObjectMatrix, modelMatrix;

            Vector3 playerPos = Player.players[0].position.pos();
            Vector3 cameraPos = Player.players[0].camera.pos();
            Vector3 cameraUp = Player.players[0].camera.up();

            if (cameraUp == Vector3.Zero)
                cameraUp = Vector3.Up;

            Matrix cameraMatrix = Matrix.CreateLookAt(cameraPos, playerPos, cameraUp);

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
            #endregion

            #region Interface rendering
            iface.Draw(new GameTime(), new Vector2(0f,0f), new Vector2(1f,1f), 0f);
            #endregion
        }
    }
}
