using System;
using System.Collections.Generic;
using System.Text;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A rotateable weapons platform attached to a racer.
    /// Models for turrets should have the *bottom* of the turret located at the origin where it will meet the ship.
    /// </summary>
    class Turret : GameObject
    {
        // The turret's location relative to the Racer's identity matrix
        public Coords location;

        // Locations to mount weapons
        public List<MountPoint> mounts;

        public Turret()
        {
            location = new Coords();
            mounts = new List<MountPoint>();
        }

        public Turret clone()
        {
            Turret turret = new Turret();
            turret.location = location;

            foreach (MountPoint mount in mounts)
            {
                turret.mounts.Add(mount.clone());
            }

            return turret;
        }
    }
}
