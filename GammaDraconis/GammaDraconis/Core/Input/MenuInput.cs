using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GammaDraconis.Core.Input
{
    /// <summary>
    /// The input manager handles translating the Xna keyboard, mouse,
    /// and gamepad data into game-usable state.
    /// </summary>
    class MenuInput : Input
    {
        public class Commands
        {
            public static String Up = "Up";
            public static String Down = "Down";
            public static String Left = "Left";
            public static String Right = "Right";
            public static String Select = "Select";
            public static String Cancel = "Cancel";
        }

        /// <summary>
        /// Initialize the Input manager's state and preferences.
        /// </summary>
        public MenuInput() : base()
        {
            // Default action assignments
            inputKeys.Add(Commands.Up, "PadUp|up");
            inputKeys.Add(Commands.Down, "PadDown|down");
            inputKeys.Add(Commands.Left, "PadLeft|left");
            inputKeys.Add(Commands.Right, "PadRight|right");
            inputKeys.Add(Commands.Select, "PadA|PadStart|enter|space");
            inputKeys.Add(Commands.Cancel, "PadB|PadBack|escape");
        }
    }
}
