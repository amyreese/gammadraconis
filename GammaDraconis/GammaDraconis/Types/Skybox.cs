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

		public Skybox() : this("Resources/Models/Skybox") {}
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
