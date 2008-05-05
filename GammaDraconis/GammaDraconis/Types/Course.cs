using System;
using System.Collections.Generic;
using System.Text;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Types
{
    class Course
    {
        // Course definition
        public List<Checkpoint> path;
        public bool loop;
        public int laps;

        public Course() : this(new Checkpoint[0], false, 1) { }
        public Course(Checkpoint[] points, bool loop, int laps)
        {
            this.loop = loop;
            this.laps = laps;
            path = new List<Checkpoint>();

            foreach (Checkpoint point in points)
            {
                path.Add(point);
            }
        }
    }
}
