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
    }
}
