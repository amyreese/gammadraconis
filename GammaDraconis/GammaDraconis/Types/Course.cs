using System;
using System.Collections.Generic;
using System.Text;
using GammaDraconis.Core;
using GammaDraconis.Video;

namespace GammaDraconis.Types
{
    class Course
    {
        // Course definition
        public List<Coords> path;
        public List<string> types;
        public bool loop;
        public int laps;

        public List<Checkpoint> checkpoints;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Course() : this(new Coords[0], new string[0], false, 1) { }

        /// <summary>
        /// Construct a course with the following parameters.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="models"></param>
        /// <param name="loop"></param>
        /// <param name="laps"></param>
        public Course(Coords[] points, string[] models, bool loop, int laps)
        {
            this.loop = loop;
            this.laps = laps;
            path = new List<Coords>();
            types = new List<string>();

            path.AddRange(points);
            types.AddRange(models);
        }

        /// <summary>
        /// Initialize the course. Construct checkpoints and add them to the lsit.
        /// </summary>
        public void init()
        {
            checkpoints = new List<Checkpoint>();

            int i = 1;
            foreach (Coords coord in path)
            {
                Checkpoint checkpoint = Checkpoint.cloneObject(Proto.getThing(types[i-1], coord.Clone()));
                checkpoint.racePosition = i++;
                Engine.GetInstance().gameScene.track(checkpoint, GO_TYPE.CHECKPOINT);
                checkpoints.Add(checkpoint);
            }
		}
    }
}
