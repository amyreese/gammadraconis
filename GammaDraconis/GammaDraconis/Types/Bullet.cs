using System;
using System.Collections.Generic;
using System.Text;
using GammaDraconis.Video;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A bullet represents a projectile fired by a weapon.
    /// </summary>
    class Bullet : GameObject
    {
        public float damage;
        public double timeToLive;
        public Coords lastPosition;

        /// <summary>
        /// Construct a bullet with default attributes.
        /// </summary>
        public Bullet()
        {
            rateL = 50f;
            rateR = 1f;
            dragL = 0f;
            dragR = 0f;
            lastPosition = position;
        }

        /// <summary>
        /// Clone another Bullet object.
        /// </summary>
        /// <returns>A cloned bullet.</returns>
        public Bullet clone()
        {
            Bullet b = new Bullet();
            
            b.damage = damage;
            b.timeToLive = timeToLive;
            b.mass = mass;
            b.size = size;
            b.lastPosition = lastPosition.Clone(); 

            b.rateL = rateL;
            b.rateR = rateR;
            b.dragL = dragL;
            b.dragR = dragR;
            foreach (FBXModel model in models)
            {
                b.models.Add(model.clone());
            }

            b.onDeathSound = onDeathSound;

            if (explosion != null)
            {
                b.explosion = explosion.clone();
            }

            return b;
        }
    }
}
