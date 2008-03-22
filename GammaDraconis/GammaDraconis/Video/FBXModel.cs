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
    class FBXModel
    {
        // The .fbx model represented by this object
        public Model model;
        public string filename;

        // Shader effect associated with the model
        public Effect _effect;
        public string effect;

        public FBXModel(string filename) : this(filename, "") { }
        public FBXModel(string filename, string effect)
        {
            this.filename = filename;
            this.effect = effect;
            offset = new Coords();
        }

        // Offset position/rotation, relative to the *game object's* identity matrix
        public Coords offset;
    }
}
