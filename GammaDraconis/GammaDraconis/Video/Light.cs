using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GammaDraconis.Types
{
    public class Light
    {
        public bool enabled;
        public Vector3 diffuse;
        public Vector3 specular;
        public Vector3 direction;

        public Light()
        {
            enabled = false;
            diffuse = new Vector3();
            specular = new Vector3();
            direction = new Vector3();
        }

        public Light(Vector3 direction, Vector3 diffuse, Vector3 specular)
        {
            enabled = true;
            this.direction = direction;
            this.diffuse = diffuse;
            this.specular = specular;
        }
        public Light(Vector3 direction, Vector3 diffuse) : this(direction, diffuse, new Vector3(0.8f, 0.8f, 0.8f)) { }
        public Light(Vector3 direction) : this(direction, new Vector3(0.6f, 0.6f, 0.6f), new Vector3(0.8f, 0.8f, 0.8f)) { }
    }
}
