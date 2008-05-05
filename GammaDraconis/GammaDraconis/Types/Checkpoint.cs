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
            size = 100f;
            racePosition = 0;
        }

        public static Checkpoint cloneObject(GameObject gameObject)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.position = gameObject.position;
            checkpoint.models = gameObject.models;
            checkpoint.size = gameObject.size;
            return checkpoint;
        }

        public Checkpoint clone()
        {
            Checkpoint go = new Checkpoint();
            go.size = size;
            go.racePosition = racePosition;
            go.position = position;
            go.models = models;

            return go;
        }
    }
}
