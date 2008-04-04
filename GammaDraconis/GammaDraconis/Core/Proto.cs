using System;
using System.Collections.Generic;
using System.Text;
using GammaDraconis.Types;

namespace GammaDraconis.Core
{
    /// <summary>
    /// Prototype library.  Instead of constantly executing Lua scripts to create
    /// game objects, just clone() an object from the Prototype library as needed.
    /// </summary>
    class Proto
    {
        public static Dictionary<string, GameObject> thing;
        public static Dictionary<string, Racer> racer;
        public static Dictionary<string, Weapon> weapon;

        /// <summary>
        /// Initialize the prototype library.
        /// </summary>
        public static void init()
        {
            thing = new Dictionary<string, GameObject>();
            racer = new Dictionary<string, Racer>();
            weapon = new Dictionary<string, Weapon>();

            GameLua lua = GammaDraconis.GetInstance().GameLua;

            // TODO: Find all lua files in the 'Objects', 'Ships', and 'Weapons'
            //       directories, and load each one o pre-create all the object
            //       definitions to be used later in the game.
        }

        /// <summary>
        /// Get a prototype clone with given name, position, and velocity.
        /// </summary>
        /// <param name="name">Prototype name</param>
        /// <param name="position">New position</param>
        /// <param name="velocity">New velocity</param>
        /// <returns></returns>
        public static GameObject Thing(string name, Coords position, Coords velocity)
        {
            GameObject go = thing[name].clone();
            go.position = position;
            go.velocity = velocity;
            return go;
        }
        public static GameObject Thing(string name, Coords position) { return Thing(name, position, new Coords()); }

        /// <summary>
        /// Get a prototype clone with given name, position, and velocity.
        /// </summary>
        /// <param name="name">Prototype name</param>
        /// <param name="position">New position</param>
        /// <param name="velocity">New velocity</param>
        /// <returns></returns>
        public static Racer Racer(string name, Coords position, Coords velocity)
        {
            Racer go = racer[name].clone();
            go.position = position;
            go.velocity = velocity;
            return go;
        }
        public static Racer Racer(string name, Coords position) { return Racer(name, position, new Coords()); }

        /// <summary>
        /// Get a prototype clone with given name, position, and velocity.
        /// </summary>
        /// <param name="name">Prototype name</param>
        /// <param name="position">New position</param>
        /// <param name="velocity">New velocity</param>
        /// <returns></returns>
        public static Weapon Weapon(string name, Coords position, Coords velocity)
        {
            Weapon go = weapon[name].clone();
            go.position = position;
            go.velocity = velocity;
            return go;
        }
        public static Weapon Weapon(string name, Coords position) { return Weapon(name, position, new Coords()); }

    }
}
