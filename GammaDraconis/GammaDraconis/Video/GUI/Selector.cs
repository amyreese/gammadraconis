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

        public Selector(GammaDraconis game)
            : base(game)
        {
            selections = new List<string>();
        }

        public Selector(GammaDraconis game, params string[] selections)
            : base(game)
        {
            this.selections = new List<string>();

            foreach (string s in selections)
                this.selections.Add(s);

            UpdateText();
        }

        public void AddSelection(string selection)
        {
            selections.Add(selection);

            if(selections.Count == 1)
                UpdateText();
        }

        public void PrevSelection()
        {
            if (--currentSel == -1)
                currentSel = selections.Count - 1;

            UpdateText();
        }

        public void NextSelection()
        {
            if (++currentSel == selections.Count)
                currentSel = 0;

            UpdateText();
        }

        private void UpdateText()
        {
            text = prevSelector + selections[currentSel] + nextSelector;
        }

        public string CurrentSelection
        {
            get { return selections[currentSel]; }
        }

        public string PrevSelector
        {
            get { return prevSelector; }
            set { prevSelector = value; }
        }

        public string NextSelector
        {
            get { return nextSelector; }
            set { nextSelector = value; }
        }
    }
}
