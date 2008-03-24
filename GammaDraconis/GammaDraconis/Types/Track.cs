using System;
using System.Collections.Generic;
using System.Text;

namespace GammaDraconis.Types
{
    class Track
    {
        // Track definition
        public List<Coords> path;
        public bool loop;

        public Track() : this(new Coords[0], false) { }
        public Track(Coords[] points, bool loop)
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
