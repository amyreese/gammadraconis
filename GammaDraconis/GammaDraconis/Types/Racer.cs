using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A Racer is a special game object that represents an intelligent
    /// competitor in races.
    /// </summary>
    class Racer : GameObject
    {
        public string name;

        // Locations to mount weapons
        public List<MountPoint> mounts;

        // Turrets for weapons
        public List<Turret> turrets;

        // Set the 3D model to draw.
        public Model model;

        // The aspect ratio determines how to scale 3d to 2d projection.
        float aspectRatio;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="game">The instance of the game.</param>
        protected Racer(GammaDraconis game) : base(game) { }

        protected override void LoadContent()
        {
            model = gammaDraconis.Content.Load<Model>("Resources\\Models\\Raptor");

            aspectRatio = (float)gammaDraconis.GraphicsDevice.Viewport.Width /
            (float)gammaDraconis.GraphicsDevice.Viewport.Height;
        } 

        // Set the position of the model in world space, and set the rotation.
        Vector3 modelPosition = Vector3.Zero;
        float modelRotation = 0.0f;

        // Set the position of the camera in world space, for our view matrix.
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 1200.0f);

        public override void Draw(GameTime gameTime)
        {
            gammaDraconis.GraphicsDevice.Clear(Color.CornflowerBlue);

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
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation)
                        * Matrix.CreateTranslation(modelPosition);
                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                        aspectRatio, 1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
