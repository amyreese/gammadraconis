using System;
using System.Collections.Generic;
using System.Text;
using GammaDraconis.Video.GUI;

namespace GammaDraconis.Types
{
    class Course
    {
        // Course definition
        public List<Coords> path;
        public bool loop;

        public Course() : this(new Coords[0], false) { }
        public Course(Coords[] points, bool loop)
        {
            this.loop = loop;
            path = new List<Coords>();

            foreach (Coords point in points)
            {
                path.Add(point);
            }
        }
    }
}
