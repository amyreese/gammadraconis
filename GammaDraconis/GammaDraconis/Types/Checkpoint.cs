using System;
using System.Collections.Generic;
using System.Text;

namespace GammaDraconis.Types
{
    class Checkpoint : GameObject
    {
        public int racePosition;

        /// <summary>
        /// Create a checkpoint with the default size.
        /// </summary>
        public Checkpoint() : base()
        {
            size = 120f;
            racePosition = 0;
        }

        /// <summary>
        /// Clone a checkpoint object.
        /// </summary>
        /// <param name="gameObject">The object to clone.</param>
        /// <returns>The cloned object.</returns>
        public static Checkpoint cloneObject(GameObject gameObject)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.position = gameObject.position;
            checkpoint.models = gameObject.models;
            checkpoint.size = gameObject.size;
            return checkpoint;
        }

        /// <summary>
        /// Clone a checkpoint object.
        /// </summary>
        /// <returns>The cloned object.</returns>
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
