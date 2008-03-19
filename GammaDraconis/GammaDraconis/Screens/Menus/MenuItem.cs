using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GammaDraconis.Screens.Menus
{
    class MenuItem : IDrawable
    {
        // The text of the menu item
        private String text;

        // The screen this menu item is on
        private MenuScreen screen;

        // The location of this menu item
        private Vector2 position;

        public readonly String command;

        /// <summary>
        /// Creates the menu item
        /// </summary>
        /// <param name="text">The text of the menu item</param>
        /// <param name="screen">The screen this menu item is on</param>
        /// <param name="position">The location of this menu item</param>
        public MenuItem(String text, MenuScreen screen, Vector2 position, String command)
        {
            this.text = text;
            this.screen = screen;
            this.position = position;
            this.command = command;
        }

        /// <summary>
        /// Moves the menu item to a new location
        /// </summary>
        /// <param name="position">The new location for the item</param>
        public void Reposition(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Draws the menu item if it is visible
        /// </summary>
        /// <param name="time">The time for this update</param>
        public void Draw(GameTime time)
        {
			if (Visible)
			{
				Color color = Selected ? screen.GetSelectedColor() : screen.GetUnselectedColor();
				float scale = Selected ? screen.GetSelectedScale() : screen.GetUnselectedScale();
				SpriteBatch spriteBatch = screen.spriteBatch;
				SpriteFont font = screen.spriteFont;

				Vector2 origin = new Vector2(0, font.LineSpacing / 2);
				spriteBatch.Begin();
				spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.End();
			}
        }

        /// <summary>
        /// Flag indicating if this item is selected or not
        /// </summary>
        private bool _selected = false;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }

        /// <summary>
        /// Flag indicating if this item is visible or not
        /// </summary>
        private bool _visible = true;
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                List<EventHandler>.Enumerator listnerEnumerator = _visibleChangedListners.GetEnumerator();
                while (listnerEnumerator.MoveNext())
                {
                    listnerEnumerator.Current.Invoke(this, EventArgs.Empty);
                }
            }
        }

        #region Required by IDrawable, but don't have an impact for our use
        private int _drawOrder = 0;
        public int DrawOrder
        {
            get
            {
                return _drawOrder;
            }
            set
            {
                _drawOrder = value;
                List<EventHandler>.Enumerator listnerEnumerator = _drawOrderChangedListners.GetEnumerator();
                while (listnerEnumerator.MoveNext())
                {
                    listnerEnumerator.Current.Invoke(this, EventArgs.Empty);
                }
            }
        }

        List<EventHandler> _drawOrderChangedListners = new List<EventHandler>();
        event EventHandler IDrawable.DrawOrderChanged
        {
            add
            {
                _drawOrderChangedListners.Add(value);
            }
            remove
            {
                _drawOrderChangedListners.Remove(value);
            }
        }

        List<EventHandler> _visibleChangedListners = new List<EventHandler>();
        event EventHandler IDrawable.VisibleChanged
        {
            add
            {
                _visibleChangedListners.Add(value);
            }
            remove
            {
                _visibleChangedListners.Remove(value);
            }
        }
        #endregion
    }
}
