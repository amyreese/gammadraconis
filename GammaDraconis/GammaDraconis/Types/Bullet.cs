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

        public Bullet()
        {
            rateL = 50f;
            rateR = 1f;
            dragL = 0f;
            dragR = 0f;

            models.Add(new FBXModel("Resources/Models/Shell", "", 0.1f));
        }

        public Bullet clone()
        {
            Bullet b = new Bullet();
            b.damage = damage;
            b.timeToLive = timeToLive;
            return b;
        }
    }
}
