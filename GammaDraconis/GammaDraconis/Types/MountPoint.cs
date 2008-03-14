using System;
using System.Collections.Generic;
using System.Text;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A position where a single weapon can mounted to a racer.
    /// </summary>
    class MountPoint
    {
        // Mount point's location relative to the Racer/Turret's identity matrix
        public Coords location;

        // The installed weapon
        public Weapon weapon;
    }
}
