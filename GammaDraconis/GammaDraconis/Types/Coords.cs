using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GammaDraconis.Types
{
    /// <summary>
    /// Represents a position / rotation coordinate in space-time.  This can 
    /// be an object's position, velocity, or acceleration.
    /// </summary>
    class Coords
    {
        // Coordinates
        public float X, Y, Z;
        public Vector3 pos()
        {
            return new Vector3(X, Y, Z);
        }

        // Rotations
        public float A, B, G;
        public Vector3 rot()
        {
            return new Vector3(A, B, G);
        }

        // Constructors
        public Coords(float x, float y, float z, float a, float b, float g)
        {
            X = x; Y = y; Z = z;
            A = a; B = b; G = g;
        }
        public Coords(float x, float y, float z) : this(x, y, z, 0, 0, 0) { }
        
    }
}
