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
        public Matrix T;

        // Rotations
        public Quaternion R;

        /// <summary>
        /// Build a Coords object with the given properties.
        /// </summary>
        /// <param name="x">X axis position</param>
        /// <param name="y">Y axis position</param>
        /// <param name="z">Z axis position</param>
        /// <param name="a">Alpha rotation (X axis / Pitch)</param>
        /// <param name="b">Beta rotation (Y axis / Yaw)</param>
        /// <param name="g">Gamma rotation (Z axis / Roll)</param>
        public Coords(float x, float y, float z, float a, float b, float g)
        {
            T = Matrix.CreateTranslation(x, y, z);
            R = Quaternion.CreateFromYawPitchRoll(b, a, g);
        }
        public Coords(float x, float y, float z) : this(x, y, z, 0, 0, 0) { }
        public Coords() : this(0, 0, 0, 0, 0, 0) {
            T = Matrix.Identity;
            R = Quaternion.Identity;
        }

        /// <summary>
        /// Get a Vector3 object containing this object's rotation.
        /// </summary>
        /// <returns>Positional Vector3</returns>
        public Vector3 pos()
        {
            return T.Translation;
        }

        /// <summary>
        /// Get a Vector3 object containing this object's up vector.
        /// </summary>
        /// <returns></returns>
        public Vector3 up()
        {
            Vector3 v = Matrix.CreateFromQuaternion(R).Up;
            return v;
        }

        /// <summary>
        /// Get a Matrix that contains both the translation and rotation of this object.
        /// </summary>
        /// <returns>Combined translation and rotation matrix</returns>
        public Matrix matrix()
        {
            return Matrix.CreateFromQuaternion(R) * T;
        }

        /// <summary>
        /// Get a camera matrix for a Coords object.
        /// </summary>
        /// <returns>Camera matrix</returns>
        public Matrix camera()
        {
            Matrix m = Matrix.CreateTranslation(0f, 0f, -1f) * Matrix.CreateFromQuaternion(R);
            return Matrix.CreateLookAt(new Vector3(0,0,0), new Vector3(0,0,-100), Vector3.Up);
        }

        /// <summary>
        /// Get a string representation of the Coords object.
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return "{{ " + R.ToString() + " || " + T.Translation.ToString() + " }}";
        }

        public Coords Clone()
        {
            Coords c = new Coords();
            c.R = new Quaternion(R.X, R.Y, R.Z, R.W);
            c.T = Matrix.Identity * T;
            return c;
        }
    }
}
