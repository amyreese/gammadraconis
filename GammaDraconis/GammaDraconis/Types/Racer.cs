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

        public Racer()
            : base()
        {
            models.Add(new FBXModel("Resources/Models/Raptor"));
            
            MountPoint mount = new MountPoint();
            mount.location = new Coords(0.2f, 0f, 0f);
            mount.weapon = new Weapon();
            mounts.Add(mount);
        }
        
        /// <summary>
        /// Clone a racer object
        /// </summary>
        /// <returns>The cloned racer</returns>
        public virtual Racer clone()
        {
            Racer go = new Racer();

            go.mass = mass;
            go.size = size;

            go.rateL = rateL;
            go.rateR = rateR;
            go.dragL = dragL;
            go.dragR = dragR;

            go.models.AddRange(models);
            
            foreach(MountPoint mount in mounts)
            {
                go.mounts.Add(mount.clone());
            }

            foreach (Turret turret in turrets)
            {
                go.turrets.Add(turret.clone());
            }

            return go;
        }
    }
}
