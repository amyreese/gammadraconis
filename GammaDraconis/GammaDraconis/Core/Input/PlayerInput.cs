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
    class PlayerInput : Input
    {
        public class Commands
        {
            public static String Up = "Up";
            public static String Down = "Down";
            public static String Left = "Left";
            public static String Right = "Right";
            public static String RollLeft = "RollLeft";
            public static String RollRight = "RollRight";
            public static String ThrottleUp = "ThrottleUp";
            public static String ThrottleDown = "ThrottleDown";
            public static String Fire1 = "Fire1";
            public static String Fire2 = "Fire2";
            public static String Pause = "Pause";
            public static String Menu = "Menu";

            public static String Yaw = "Yaw";
            public static String Pitch = "Pitch";
            public static String Roll = "Roll";
            public static String Turn = "Turn"; 
            public static String Throttle = "Throttle";

            public static String CameraX = "CameraX";
            public static String CameraY = "CameraY";
        }

        /// <summary>
        /// Initialize the Input manager's state and preferences.
        /// </summary>
        public PlayerInput( PlayerIndex playerIndex )
            : base(playerIndex)
        {
            // Default action assignments
            if (GamePad.GetCapabilities(playerIndex).IsConnected)
            {
                inputAxis.Add(Commands.Turn, "LeftX");
                inputAxis.Add(Commands.Pitch, "LeftY");
                inputAxis.Add(Commands.Throttle, "Triggers");
                inputAxis.Add(Commands.CameraX, "RightX");
                inputAxis.Add(Commands.CameraY, "RightY");

                inputKeys.Add(Commands.RollLeft, "PadLB");
                inputKeys.Add(Commands.RollRight, "PadRB");

                inputKeys.Add(Commands.Fire1, "PadA");
                inputKeys.Add(Commands.Fire2, "PadB");
                inputKeys.Add(Commands.Pause, "PadStart");
                inputKeys.Add(Commands.Menu, "PadBack");

            }
            else
            {
                /*
                    inputKeys.Add(Commands.Up, "up");
                    inputKeys.Add(Commands.Down, "down");
                    inputKeys.Add(Commands.Left, "left");
                    inputKeys.Add(Commands.Right, "right");
                    inputKeys.Add(Commands.YawLeft, "q");
                    inputKeys.Add(Commands.YawRight, "w");
                    inputKeys.Add(Commands.ThrottleUp, "a");
                    inputKeys.Add(Commands.ThrottleDown, "z");
                    inputKeys.Add(Commands.Fire1, "space");
                    inputKeys.Add(Commands.Fire2, "enter");
                */
                if (playerIndex == PlayerIndex.One)
                {
                    inputKeys.Add(Commands.Up, "up");
                    inputKeys.Add(Commands.Down, "down");
                    inputKeys.Add(Commands.Left, "left");
                    inputKeys.Add(Commands.Right, "right");
                    inputKeys.Add(Commands.RollLeft, "a");
                    inputKeys.Add(Commands.RollRight, "d");
                    inputKeys.Add(Commands.ThrottleUp, "w");
                    inputKeys.Add(Commands.ThrottleDown, "s");
                    inputKeys.Add(Commands.Fire1, "space");
                    inputKeys.Add(Commands.Fire2, "enter");
                }
                else
                {
                    inputKeys.Add(Commands.Up, "numpad8");
                    inputKeys.Add(Commands.Down, "numpad2");
                    inputKeys.Add(Commands.Left, "numpad4");
                    inputKeys.Add(Commands.Right, "numpad6");
                    inputKeys.Add(Commands.RollLeft, "numpad7");
                    inputKeys.Add(Commands.RollRight, "numpad9");
                    inputKeys.Add(Commands.ThrottleUp, "numpad5");
                    inputKeys.Add(Commands.ThrottleDown, "numpad0");
                    inputKeys.Add(Commands.Fire1, "numpad1");
                    inputKeys.Add(Commands.Fire2, "numpad3");
                }
                inputKeys.Add(Commands.Pause, "p");
                inputKeys.Add(Commands.Menu, "escape");
            }
        }
    }
}
