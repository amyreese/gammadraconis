using System;
using System.Collections.Generic;
using System.Text;
using GammaDraconis.Video;

namespace GammaDraconis.Types
{
    class Skybox : GameObject
    {
		public Skybox() : base()
		{
			models.Add(new FBXModel("Resources/Models/Skybox", "", 0.195f * 3));
		}
    }
}
