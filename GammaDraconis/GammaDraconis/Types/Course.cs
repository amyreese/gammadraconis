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

        public Course() : this(new Checkpoint[0], false) { }
        public Course(Checkpoint[] points, bool loop)
        {
            this.loop = loop;
            path = new List<Checkpoint>();

            foreach (Checkpoint point in points)
            {
                path.Add(point);
            }
        }
    }
}
