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
		public int type = W_TYPE.PRIMARY;
        public Weapon weapon;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MountPoint()
        {
            location = new Coords();
        }

        /// <summary>
        /// Clone a MountPoint object.
        /// </summary>
        /// <returns>A cloned MountPoint object.</returns>
        public MountPoint clone()
        {
            MountPoint mount = new MountPoint();
			mount.type = type;
            mount.location = location;
            if (weapon != null)
            {
                mount.weapon = weapon.clone();
            }

            return mount;
        }
    }
}
