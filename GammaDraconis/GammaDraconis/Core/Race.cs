using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GammaDraconis.Types;

namespace GammaDraconis.Core
{
    /// <summary>
    /// Manages a race, given a set of Racers and a Course.
    /// </summary>
    class Race
    {
        // The course for the race
        private Course course;
        private int length;
        private bool loop;

        // The racers and their statuses
        private Dictionary<Racer, int> state;

        /// <summary>
        /// Create a race manager.
        /// </summary>
        /// <param name="course">The race course</param>
        /// <param name="racers">The racers</param>
        public Race(Course course, List<Racer> racers)
        {
            this.course = course;
            length = course.path.Count;
            loop = course.loop;

            foreach (Racer racer in racers)
            {
                state.Add(racer, 0);
            }
        }

        /// <summary>
        /// Get an upcoming coordinate for the given racer.
        /// </summary>
        /// <param name="racer">The racer object</param>
        /// <param name="offset">How many coordinates forward in the race</param>
        /// <returns></returns>
        public Coords coord(Racer racer, int offset)
        {
            int[] info = status(racer);
            return new Coords();
        }
        public Coords nextCoord(Racer racer) { return coord(racer, 1); }

        /// <summary>
        /// Update the current status of the Race manager.
        /// </summary>
        public void update()
        {
        }

        /// <summary>
        /// Get a the lap and checkpoint 
        /// </summary>
        /// <param name="racer"></param>
        /// <returns></returns>
        private int[] status(Racer racer)
        {
            int status = state[racer];
            int[] info = new int[2];
            info[0] = status / length;
            info[1] = status % length;
            return info;
        }
    }
}
