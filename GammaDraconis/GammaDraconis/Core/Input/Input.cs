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
        protected Dictionary<String, String> inputKeys;
        private Dictionary<String, KeyState> keyStates;
        private Dictionary<String, KeyState> keyStatesOld;
        private Dictionary<String, Boolean> keyPresses;

        protected PlayerIndex playerIndex;

        public Vector2 mousePosition;

        /// <summary>
        /// Initialize the Input manager's state and preferences.
        /// </summary>
        public Input()
        {
            inputKeys = new Dictionary<String, String>();
            keyStates = new Dictionary<String, KeyState>();
            keyPresses = new Dictionary<String, Boolean>();
        }

        /// <summary>
        /// Check the state of an input action.
        /// </summary>
        /// <param name="action">The input action.</param>
        /// <returns>Whether or not the key that performs this action is down.</returns>
        public Boolean inputDown(String action)
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
        public Boolean inputPressed(String action)
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
        /// Update the input states.
        /// </summary>
        public void update()
        {
            // Shift keyStates to keyStatesOld
            foreach(String key in inputKeys.Values)
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
                String keyName = pressedKeys[pressedKeyIndex].ToString();

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
                String keyName = pressedKeys[pressedKeyIndex].ToString().ToLower();
                if (keyName == "back")
                {
                    keyName = "backspace";
                }
                if (keyName == "oemcomma")
                {
                    keyName = "comma";
                }

                String modName = (control ? "control+" : "") + (alt ? "alt+" : "") + (shift ? "shift+" : "") + keyName;
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

            if (keyStates.ContainsKey("Mouse1"))
            {
                keyStates["Mouse1"] = (mouseState.LeftButton == ButtonState.Pressed ? KeyState.Down : KeyState.Up);
            }

            if (keyStates.ContainsKey("Mouse2"))
            {
                keyStates["Mouse2"] = (mouseState.RightButton == ButtonState.Pressed ? KeyState.Down : KeyState.Up);
            }

            if (keyStates.ContainsKey("Mouse3"))
            {
                keyStates["Mouse3"] = (mouseState.MiddleButton == ButtonState.Pressed ? KeyState.Down : KeyState.Up);
            }

            // Find new key presses
            foreach (String key in inputKeys.Values)
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
            keyStates = new Dictionary<String, KeyState>();
            keyStatesOld = new Dictionary<String, KeyState>();
            keyPresses = new Dictionary<String, Boolean>();

            // Loop through all defined input actions
            foreach(String key in inputKeys.Values)
            {
                // Set all inputs to up
                keyStates.Add(key, KeyState.Up);
                keyStatesOld.Add(key, KeyState.Up);

                // Set all inputs as not pressed
                keyPresses.Add(key, false);
            }
        }

        /// <summary>
        /// Get the key binding that corresponds to the given action.
        /// </summary>
        /// <param name="action">An action.</param>
        /// <returns>The key binding.</returns>
        public String getKeyBinding(String action)
        {
            return inputKeys[action];
        }
    }
}
