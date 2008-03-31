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
        private int laps;

        // The racers and their statuses
        private Dictionary<Racer, int> state;

        /// <summary>
        /// Create a race manager.
        /// </summary>
        /// <param name="course">The race course</param>
        /// <param name="racers">The racers</param>
        public Race(Course course, int laps, Racer[] racers)
        {
            this.course = course;
            this.laps = (course.loop ? laps : 1);
            state = new Dictionary<Racer, int>();
            foreach (Racer racer in racers)
            {
                state.Add(racer, -1);
            }
        }

        /// <summary>
        /// Get an upcoming coordinate for the given racer.
        /// </summary>
        /// <param name="racer">The racer object</param>
        /// <param name="offset">How many coordinates forward in the race</param>
        /// <returns>Coordinate object, or Null if past end of race</returns>
        public Coords coord(Racer racer, int offset)
        {
            RaceStatus info = status(racer);
            int lap = info.lap;
            int point = info.checkpoint + offset;

            while (point >= course.path.Count)
            {
                if (!course.loop || lap >= laps)
                {
                    return null;
                }

                lap++;
                point -= course.path.Count;
            }

            return course.path[point];
        }
        public Coords nextCoord(Racer racer) { 
            return coord(racer, 1); 
        }

        /// <summary>
        /// See if there is a winner yet.
        /// </summary>
        /// <returns>Null if no winner, Racer object otherwise</returns>
        public Racer winner()
        {
            return null;
        }

        /// <summary>
        /// Update the current status of the Race manager.
        /// </summary>
        public void update()
        {
            foreach (Racer r in new List<Racer>(state.Keys))
            {
                BoundingSphere checkpointSphere = new BoundingSphere(nextCoord(r).pos(),25.0f);
                BoundingSphere racerSphere = new BoundingSphere(r.position.pos(), 10.0f);
                if (checkpointSphere.Intersects(racerSphere))
                {
                    state[r] += 1;
                }
            }
        }

        /// <summary>
        /// Get a the lap and checkpoint 
        /// </summary>
        /// <param name="racer"></param>
        /// <returns></returns>
        public RaceStatus status(Racer racer)
        {
            int status = state[racer];
            RaceStatus raceStatus = new RaceStatus();
            raceStatus.lap = status / course.path.Count;
            raceStatus.checkpoint = status % course.path.Count;
            return raceStatus;
        }
    }

    public struct RaceStatus
    {
        public int lap;
        public int checkpoint;
    }
}
