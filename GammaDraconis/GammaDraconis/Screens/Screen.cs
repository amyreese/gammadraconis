using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GammaDraconis.Screens
{
    /// <summary>
    /// The base that all screens displayed to the user inherit from
    /// </summary>
    abstract class Screen : DrawableGameComponent
    {
        /// <summary>
        /// The instance of snails pace that is using this screen
        /// </summary>
        protected GammaDraconis gammaDraconis;

        /// <summary>
        /// Creates a new Screen
        /// </summary>
        /// <param name="game">The instance of Snails Pace</param>
        protected Screen(GammaDraconis game)
            : base(game)
        {
            gammaDraconis = game;
        }

        /// <summary>
        /// Used as a flag to determine if we can transition to or from this screen or not
        /// </summary>
        private bool _ready = false;
        public bool ready
        {
            get
            {
                return _ready;
            }
            set
            {
                _ready = value;
            }
        }
    }
}
