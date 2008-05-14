using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GammaDraconis.Core;
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
        }

        public override void throttle(float amount)
        {
            amount *= 1f - 0.06f * (Engine.GetInstance().race.status(this).leading);
            amount = -MathHelper.Clamp(amount, -1f, 1f);
            acceleration.T *= Matrix.CreateTranslation(0f, 0f, amount);
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

            foreach (FBXModel model in models)
            {
                go.models.Add(model.clone());
            }

            if (shieldModel != null)
            {
                go.shieldModel = shieldModel.clone();
            }
            
            foreach(MountPoint mount in mounts)
            {
                go.mounts.Add(mount.clone());
            }

            foreach (Turret turret in turrets)
            {
                go.turrets.Add(turret.clone());
            }

            go.relativeLookAt = new Vector3(relativeLookAt.X, relativeLookAt.Y, relativeLookAt.Z);
            go.relativeLookFrom = new Vector3(relativeLookFrom.X, relativeLookFrom.Y, relativeLookFrom.Z);
            go.relationalScale = relationalScale;

            return go;
        }

        /// <summary>
        /// Create Racer object from ship definition.
        /// </summary>
        /// <param name="ship">The target ship</param>
        /// <returns>New Racer object</returns>
        public static Racer cloneShip(GameObject ship)
        {
            Racer go = new Racer();

            go.mass = ship.mass;
            go.size = ship.size;

            go.rateL = ship.rateL;
            go.rateR = ship.rateR;
            go.dragL = ship.dragL;
            go.dragR = ship.dragR;

            foreach (FBXModel model in ship.models)
            {
                go.models.Add(model.clone());
            }

            if (ship.shieldModel != null)
            {
                go.shieldModel = ship.shieldModel.clone();
            }
            
            foreach(MountPoint mount in ship.mounts)
            {
                go.mounts.Add(mount.clone());
            }

            foreach (Turret turret in ship.turrets)
            {
                go.turrets.Add(turret.clone());
            }

            go.relativeLookAt = new Vector3(ship.relativeLookAt.X, ship.relativeLookAt.Y, ship.relativeLookAt.Z);
            go.relativeLookFrom = new Vector3(ship.relativeLookFrom.X, ship.relativeLookFrom.Y, ship.relativeLookFrom.Z);
            go.relationalScale = ship.relationalScale;

            return go;
        }
    }
}
