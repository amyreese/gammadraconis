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

        public Bullet()
        {
            rateL = 2000f;
            rateR = 200f;
            dragL = 0f;
            dragR = 0f;

            models.Add(new FBXModel("Resources/Models/Checkpoint", "", 0.1f));
        }

        public Bullet clone()
        {
            return new Bullet();
        }
    }
}
