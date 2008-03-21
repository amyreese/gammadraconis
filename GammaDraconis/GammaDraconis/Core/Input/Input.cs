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
    class Input
    {
        protected Dictionary<string, string> inputKeys;
        protected Dictionary<string, string> inputAxis;

        private Dictionary<string, KeyState> keyStates;
        private Dictionary<string, KeyState> keyStatesOld;
        private Dictionary<string, bool> keyPresses;
        private Dictionary<string, float> axisState;

        protected bool useGamepad;
        protected PlayerIndex playerIndex;

        public Vector2 mousePosition;

        /// <summary>
        /// Initialize the Input manager's state and preferences.
        /// </summary>
        /// <param name="index">Game controller index</param>
        public Input( PlayerIndex index )
        {
            inputKeys = new Dictionary<string, string>();
            inputAxis = new Dictionary<string, string>();
            
            useGamepad = true;
            playerIndex = index;
        }

        /// <summary>
        /// Initialize the input manager without using a gamepad.
        /// </summary>
        public Input()
        {
            inputKeys = new Dictionary<string, string>();
            
            useGamepad = false;
        }

        /// <summary>
        /// Check the state of an input action.
        /// </summary>
        /// <param name="action">The input action.</param>
        /// <returns>Whether or not the key that performs this action is down.</returns>
        public Boolean inputDown(string action)
        {
            if (inputKeys.ContainsKey(action))
            {
                return keyStates[inputKeys[action]] == KeyState.Down;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the action's input has been pressed.  
        /// Distinct from inputState() in that it only returns True once for each time the key goes down.
        /// </summary>
        /// <param name="action">The input action.</param>
        /// <returns>Whether or not the key that performs this action is pressed.</returns>
        public Boolean inputPressed(string action)
        {
            if (inputKeys.ContainsKey(action))
            {
                Boolean pressed = keyPresses[inputKeys[action]];
                keyPresses[inputKeys[action]] = false;
                return pressed;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the current position of a specific controller axis. 
        /// </summary>
        /// <param name="action">The input action</param>
        /// <returns>Floating point axis position</returns>
        public float axis(string action)
        {
            if (inputAxis.ContainsKey(action))
            {
                return axisState[inputAxis[action]];
            }
            else
            {
                return 0f;
            }
        }

        /// <summary>
        /// Update the input states.
        /// </summary>
        public void update()
        {
            // Shift keyStates to keyStatesOld
            foreach(string key in inputKeys.Values)
            {
                keyStatesOld[key] = keyStates[key];
                keyStates[key] = KeyState.Up;
            }
            
            // Check keyboard inputs
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();

            bool control = false;
            bool shift = false;
            bool alt = false;

            // Handle modifier keys
            for (int pressedKeyIndex = 0; pressedKeyIndex < pressedKeys.Length; pressedKeyIndex++)
            {
                string keyName = pressedKeys[pressedKeyIndex].ToString();

                if (keyName == "LeftAlt" || keyName == "RightAlt")
                {
                    alt = true;
                }

                if (keyName == "LeftShift" || keyName == "RightShift")
                {
                    shift = true;
                }

                if (keyName == "LeftControl" || keyName == "RightControl")
                {
                    control = true;
                }
            }

            // Handle stupid MS key names
            for (int pressedKeyIndex = 0; pressedKeyIndex < pressedKeys.Length; pressedKeyIndex++)
            {
                string keyName = pressedKeys[pressedKeyIndex].ToString().ToLower();
                if (keyName == "back")
                {
                    keyName = "backspace";
                }
                if (keyName == "oemcomma")
                {
                    keyName = "comma";
                }

                string modName = (control ? "control+" : "") + (alt ? "alt+" : "") + (shift ? "shift+" : "") + keyName;
                if (keyStates.ContainsKey(keyName))
                {
                    keyStates[keyName] = KeyState.Down;
                }
                if (keyStates.ContainsKey(modName))
                {
                    keyStates[modName] = KeyState.Down;
                }
            }

            // Check mouse inputs
            MouseState mouseState = Mouse.GetState();

            mousePosition = new Vector2(mouseState.X, mouseState.Y);

            buttonState("Mouse1", mouseState.LeftButton);
            buttonState("Mouse2", mouseState.RightButton);
            buttonState("Mouse3", mouseState.MiddleButton);

            #region GamePad state handling
            if (useGamepad && GamePad.GetCapabilities(playerIndex).IsConnected 
                && GamePad.GetCapabilities(playerIndex).GamePadType == GamePadType.GamePad)
            {
                GamePadState gamepadState = GamePad.GetState(playerIndex);

                buttonState("PadUp", gamepadState.DPad.Up);
                buttonState("PadDown", gamepadState.DPad.Down);
                buttonState("PadLeft", gamepadState.DPad.Left);
                buttonState("PadRight", gamepadState.DPad.Right);
                
                buttonState("PadA", gamepadState.Buttons.A);
                buttonState("PadB", gamepadState.Buttons.B);
                buttonState("PadX", gamepadState.Buttons.X);
                buttonState("PadY", gamepadState.Buttons.Y);
                buttonState("PadLB", gamepadState.Buttons.LeftShoulder);
                buttonState("PadRB", gamepadState.Buttons.RightShoulder);
                buttonState("PadLS", gamepadState.Buttons.LeftStick);
                buttonState("PadRS", gamepadState.Buttons.RightStick);
                buttonState("PadBack", gamepadState.Buttons.Back);
                buttonState("PadStart", gamepadState.Buttons.Start);

                axisState["LeftX"] = gamepadState.ThumbSticks.Left.X;
                axisState["LeftY"] = gamepadState.ThumbSticks.Left.Y;
                axisState["RightX"] = gamepadState.ThumbSticks.Right.X;
                axisState["RightY"] = gamepadState.ThumbSticks.Right.Y;
                axisState["LeftT"] = gamepadState.Triggers.Left;
                axisState["RightT"] = gamepadState.Triggers.Right;
                axisState["Triggers"] = (gamepadState.Triggers.Right - gamepadState.Triggers.Left);
            }
            #endregion

            // Find new key presses
            foreach (string key in inputKeys.Values)
            {
                keyPresses[key] =
                    (keyPresses[key] || (keyStates[key] == KeyState.Down) && (keyStatesOld[key] == KeyState.Up))
                    && (keyStates[key] == KeyState.Down);
            }
        }

        /// <summary>
        /// Reset keyStates and keyPresses.
        /// </summary>
        public void reset()
        {
            // Reset input dictionaries
            keyStates = new Dictionary<string, KeyState>();
            keyStatesOld = new Dictionary<string, KeyState>();
            keyPresses = new Dictionary<string, bool>();
            
            // Loop through all defined input actions
            foreach(string key in inputKeys.Values)
            {
                // Set all inputs to up
                keyStates.Add(key, KeyState.Up);
                keyStatesOld.Add(key, KeyState.Up);

                // Set all inputs as not pressed
                keyPresses.Add(key, false);
            }

            // Gamepad handling
            axisState = new Dictionary<string, float>();
            axisState.Add("LeftX", 0f);
            axisState.Add("LeftY", 0f);
            axisState.Add("RightX", 0f);
            axisState.Add("RightY", 0f);
            axisState.Add("LeftT", 0f);
            axisState.Add("RightT", 0f);
            axisState.Add("Triggers", 0f);
        }

        /// <summary>
        /// Get the key binding that corresponds to the given action.
        /// </summary>
        /// <param name="action">An action.</param>
        /// <returns>The key binding.</returns>
        public string getKeyBinding(string action)
        {
            return inputKeys[action];
        }


        private void buttonState(string buttonName, ButtonState buttonState)
        {
            if (keyStates.ContainsKey(buttonName))
            {
                keyStates[buttonName] = (buttonState == ButtonState.Pressed ? KeyState.Down : KeyState.Up);
            }
        }
    }
}
