using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Video;

namespace GammaDraconis.Types
{
    class Skybox : GameObject
    {
        public static Vector3 ambient;
        public static Light[] lights;

        /// <summary>
        /// Create a new skybox with the default texture.
        /// </summary>
		public Skybox() : this("Resources/Models/Skybox") {}

        /// <summary>
        /// Create a new skybox with the specified texture.
        /// </summary>
        /// <param name="texture">The texture to map on the skybox.</param>
        public Skybox(string texture) : base()
        {
            FBXModel fbx = new FBXModel(texture, "", 0.195f * 3);
            fbx.lighted = false;
			models.Add(fbx);

            ambient = new Vector3(0.2f, 0.2f, 0.2f);
            if (lights == null)
            {
                lights = new Light[3];
            }
		}
    }
}
