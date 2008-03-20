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
        Vector3 cameraPosition = new Vector3(0.0f, 150.0f, 1200.0f);

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
            List<GameObject> objects = scene.visibleObjects();

            game.GraphicsDevice.Clear(Color.Black);
            game.GraphicsDevice.RenderState.DepthBufferEnable = true;

            Matrix worldMatrix = Matrix.Identity;
            Matrix objectMatrix, subObjectMatrix, modelMatrix;

            foreach (GameObject gameObject in objects)
            {
                objectMatrix = worldMatrix 
                    * Matrix.CreateTranslation(gameObject.position.pos()) 
                    * Matrix.CreateFromYawPitchRoll(gameObject.position.A, gameObject.position.B, gameObject.position.G);


                foreach (FBXModel fbxmodel in gameObject.models)
                {
                    modelMatrix = objectMatrix
                        * Matrix.CreateTranslation(fbxmodel.offset.pos())
                        * Matrix.CreateFromYawPitchRoll(fbxmodel.offset.A, fbxmodel.offset.B, fbxmodel.offset.G);

                    Model model = fbxmodel.model;

                    if (model == null)
                    {
                        model = game.Content.Load<Model>(fbxmodel.filename);
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
                            effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60.0f),
                                aspectRatio, 1.0f, 10000.0f);
                        }
                        // Draw the mesh, using the effects set above.
                        mesh.Draw();
                    }
                }
            }
        }
    }
}
