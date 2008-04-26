using System;
using System.Collections.Generic;
using System.Text;

namespace GammaDraconis.Types
{
    class Checkpoint : GameObject
    {
        public int racePosition;
        public Checkpoint() : base()
        {
            size = 50.0f;
            racePosition = 0;
        }

        public static Checkpoint cloneObject(GameObject gameObject)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.position = gameObject.position;
            checkpoint.models = gameObject.models;
            return checkpoint;
        }
    }
}
