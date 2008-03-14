using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Types;

namespace GammaDraconis.Video
{
    /// <summary>
    /// A model contains all the information needed to represent a 
    /// three-dimensional mesh that can be drawn by the renderer.
    /// A model should also contain its own collision mesh.
    /// </summary>
    class Model
    {
        // Offset position/rotation, relative to the *game object's* identity matrix
        public Coords offset;
    }
}
