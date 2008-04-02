using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Video;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A Racer is a special game object that represents an intelligent
    /// competitor in races.
    /// </summary>
    class Racer : GameObject
    {
        public string name;

        public Racer(GammaDraconis game)
            : base()
        {
            models.Add(new FBXModel("Resources/Models/Raptor"));
            
            MountPoint mount = new MountPoint();
            mount.location = new Coords(0.2f, 0f, 0f);
            mount.weapon = new Weapon();
            mounts.Add(mount);
        }
    }
}
