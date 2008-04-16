using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GammaDraconis.Video
{
    class Room
    {
        public bool canSeeOutside;
        public List<Room> visibleRooms = new List<Room>();
        public BoundingBox area;
    }
}
