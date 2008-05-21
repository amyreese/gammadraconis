using System;
using System.Collections.Generic;
using System.Text;

namespace GammaDraconis.Video.GUI
{
    class Selector : Text
    {
        private List<string> selections;
        private int currentSel = 0;
        private string prevSelector = "< ";
        private string nextSelector = " >";

        /// <summary>
        /// Create a new selector.
        /// </summary>
        /// <param name="game">The game instance.</param>
        public Selector(GammaDraconis game)
            : base(game)
        {
            selections = new List<string>();
        }

        /// <summary>
        /// Create a new selector.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="selections">A list of selections.</param>
        public Selector(GammaDraconis game, params string[] selections)
            : base(game)
        {
            this.selections = new List<string>();

            foreach (string s in selections)
                this.selections.Add(s);

            UpdateText();
        }

        /// <summary>
        /// Create a new selector.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="selections">A list of selections.</param>
        public Selector(GammaDraconis game, IEnumerable<string> selections)
            : base(game)
        {
            this.selections = new List<string>(selections);

            UpdateText();
        }

        /// <summary>
        /// Add a selection to this end of this list.
        /// </summary>
        /// <param name="selection"></param>
        public void AddSelection(string selection)
        {
            selections.Add(selection);

            if(selections.Count == 1)
                UpdateText();
        }

        /// <summary>
        /// Go to the previous list item.
        /// </summary>
        public void PrevSelection()
        {
            if (--currentSel == -1)
                currentSel = selections.Count - 1;

            UpdateText();
        }

        /// <summary>
        /// Go to the next list item.
        /// </summary>
        public void NextSelection()
        {
            if (++currentSel == selections.Count)
                currentSel = 0;

            UpdateText();
        }

        /// <summary>
        /// Update the text that is being displayed.
        /// </summary>
        private void UpdateText()
        {
            text = prevSelector + selections[currentSel] + nextSelector;
        }

        /// <summary>
        /// Get the currently selected item.
        /// </summary>
        public string CurrentSelection
        {
            get { return selections[currentSel]; }
        }

        /// <summary>
        /// Get or set the previous selector string, typically "&lt; ".
        /// </summary>
        public string PrevSelector
        {
            get { return prevSelector; }
            set { prevSelector = value; UpdateText(); }
        }

        /// <summary>
        /// Get or set the next selector string, typically " &gt;".
        /// </summary>
        public string NextSelector
        {
            get { return nextSelector; }
            set { nextSelector = value; UpdateText(); }
        }
    }
}
