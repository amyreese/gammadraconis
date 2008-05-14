using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GammaDraconis.Video
{
    /// <summary>
    /// A simple class that holds data about a room. Used by the scene manager.
    /// </summary>
    class Room
    {
        public bool canSeeOutside;
        public List<Room> visibleRooms = new List<Room>();
        public BoundingBox area;
    }
}
