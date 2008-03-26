using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Types;

namespace GammaDraconis.Video
{
    /// <summary>
    /// A model contains all the information needed to represent a 
    /// three-dimensional mesh that can be drawn by the renderer.
    /// A model should also contain its own collision mesh.
    /// </summary>
    class FBXModel : DrawableGameComponent
    {
        // The .fbx model represented by this object
        public Model model;
        public string filename;

        // Shader effect associated with the model
        public Effect _effect;
        public string effect;

        // Model scaling
        public float scale;

        public FBXModel(GammaDraconis game, string filename) : this(game, filename, "") { }
        public FBXModel(GammaDraconis game, string filename, string effect) : this(game, filename, effect, 1f) { }
        public FBXModel(GammaDraconis game, string filename, string effect, float scale) : base(game)
        {
            this.filename = filename;
            this.effect = effect;
            this.scale = scale;
            offset = new Coords();
            game.Components.Add(this);
        }

        // Offset position/rotation, relative to the *game object's* identity matrix
        public Coords offset;

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>(filename);
            base.LoadContent();
        }
    }
}
