using System;
using System.IO;
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
        public static Dictionary<string, GameObject> ship;
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

            // Load weapon prototypes
            if (Directory.Exists("Lua/Weapons"))
            {
                Console.WriteLine("Loading Weapons...");
                string[] files = Directory.GetFiles("Lua/Weapons", "*.lua", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    lua.DoFile(file);
                }
            }

            // Load thing prototypes
            if (Directory.Exists("Lua/Things"))
            {
                Console.WriteLine("Loading Things...");
                string[] files = Directory.GetFiles("Lua/Things", "*.lua", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    lua.DoFile(file);
                }
            }

            // Load racer protoypes
            if (Directory.Exists("Lua/Ships"))
            {
                Console.WriteLine("Loading Ships...");
                string[] files = Directory.GetFiles("Lua/Ships", "*.lua", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    lua.DoFile(file);
                }
            }
        }

        /// <summary>
        /// Get a prototype clone with given name, position, and velocity.
        /// </summary>
        /// <param name="name">Prototype name</param>
        /// <param name="position">New position</param>
        /// <param name="velocity">New velocity</param>
        /// <returns></returns>
        public static GameObject getThing(string name, Coords position, Coords velocity)
        {
            if (thing.ContainsKey(name))
            {
                GameObject go = thing[name].clone();
                go.position = position;
                go.velocity = velocity;
                return go;
            }
            else
            {
                return null;
            }
        }
        public static GameObject getThing(string name, Coords position) { return getThing(name, position, new Coords()); }

        /// <summary>
        /// Get a prototype clone with given name, position, and velocity.
        /// </summary>
        /// <param name="name">Prototype name</param>
        /// <param name="position">New position</param>
        /// <param name="velocity">New velocity</param>
        /// <returns></returns>
        public static GameObject getShip(string name, Coords position, Coords velocity)
        {
            if (ship.ContainsKey(name))
            {
                GameObject go = ship[name].clone();
                go.position = position;
                go.velocity = velocity;
                return go;
            }
            else
            {
                return null;
            }
        }
        public static GameObject getShip(string name, Coords position) { return getShip(name, position, new Coords()); }

        /// <summary>
        /// Get a prototype clone with given name, position, and velocity.
        /// </summary>
        /// <param name="name">Prototype name</param>
        /// <param name="position">New position</param>
        /// <param name="velocity">New velocity</param>
        /// <returns></returns>
        public static Racer getRacer(string name, Coords position, Coords velocity)
        {
            if (racer.ContainsKey(name))
            {
                Racer go = racer[name].clone();
                go.position = position;
                go.velocity = velocity;
                return go;
            }
            else
            {
                if (ship.ContainsKey(name))
                {
                    return Racer.cloneShip(ship[name]);
                }
                return null;
            }
        }
        public static Racer getRacer(string name, Coords position) { return getRacer(name, position, new Coords()); }

        /// <summary>
        /// Get a prototype clone with given name, position, and velocity.
        /// </summary>
        /// <param name="name">Prototype name</param>
        /// <param name="position">New position</param>
        /// <param name="velocity">New velocity</param>
        /// <returns></returns>
        public static Weapon getWeapon(string name, Coords position, Coords velocity)
        {
            if (weapon.ContainsKey(name))
            {
                Weapon go = weapon[name].clone();
                go.position = position;
                go.velocity = velocity;
                return go;
            }
            else
            {
                return null;
            }
        }
        public static Weapon getWeapon(string name, Coords position) { return getWeapon(name, position, new Coords()); }

    }
}
