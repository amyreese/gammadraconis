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

        static private List<string> keys;
        static private Dictionary<string, KeyState> keyStates;
        static private Dictionary<string, KeyState> keyStatesOld;
        static private Dictionary<string, bool> keyPresses;
        static private Dictionary<string, float> axisState;

        static private bool inputReset = false;

        protected bool oneGamepad;
        protected PlayerIndex playerIndex;

        static public Vector2 mousePosition;

        /// <summary>
        /// Initialize the Input manager's state and preferences.
        /// </summary>
        /// <param name="index">Game controller index</param>
        public Input( PlayerIndex index )
        {
            if (!Input.inputReset)
            {
                Input.reset();
            }

            inputKeys = new Dictionary<string, string>();
            inputAxis = new Dictionary<string, string>();
            
            oneGamepad = true;
            playerIndex = index;
        }

        /// <summary>
        /// Initialize the input manager without using a gamepad.
        /// </summary>
        public Input()
        {
            if (!Input.inputReset)
            {
                Input.reset();
            }

            inputKeys = new Dictionary<string, string>();
            inputAxis = new Dictionary<string, string>();
            
            oneGamepad = false;
        }

        /// <summary>
        /// Check the state of an input action.
        /// </summary>
        /// <param name="action">The input action.</param>
        /// <returns>Whether or not the key that performs this action is down.</returns>
        public bool inputDown(string action)
        {
            if (inputKeys.ContainsKey(action))
            {
                string[] keys = inputKeys[action].Split('|');

                foreach (string key in keys)
                {
                    if (keyStates.ContainsKey(key)) // keyboard/mouse
                    {
                        return keyStates[key] == KeyState.Down;
                    }
                    else // gamepad
                    {
                        PlayerIndex istart = oneGamepad ? playerIndex : PlayerIndex.One;
                        PlayerIndex iend = oneGamepad ? playerIndex : PlayerIndex.Four;

                        for (PlayerIndex index = istart; index <= iend; index++)
                        {
                            string gp = "Pad" + (int)index + "-";
                            if (keyStates.ContainsKey(gp + key) && keyStates[gp + key] == KeyState.Down)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            
            return false;
        }

        /// <summary>
        /// Check if the action's input has been pressed.  
        /// Distinct from inputState() in that it only returns True once for each time the key goes down.
        /// </summary>
        /// <param name="action">The input action.</param>
        /// <returns>Whether or not the key that performs this action is pressed.</returns>
        public bool inputPressed(string action)
        {
            if (inputKeys.ContainsKey(action))
            {
                string[] keys = inputKeys[action].Split('|');

                foreach (string key in keys)
                {
                    if (keyPresses.ContainsKey(key)) // keyboard/mouse
                    {
                        if (keyPresses[key])
                        {
                            keyPresses[key] = false;
                            return true;
                        }
                    }
                    else // gamepad
                    {
                        PlayerIndex istart = oneGamepad ? playerIndex : PlayerIndex.One;
                        PlayerIndex iend = oneGamepad ? playerIndex : PlayerIndex.Four;
                        for (PlayerIndex index = istart; index <= iend; index++)
                        {
                            string gp = "Pad" + (int)index + "-";
                            if (keyPresses.ContainsKey(gp + key) && keyPresses[gp + key])
                            {
                                keyPresses[gp + key] = false;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
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
                string axis = inputAxis[action];
                if (axisState.ContainsKey(axis))
                {
                    return axisState[axis];
                }
                string gp = "Pad" + (int)playerIndex + "-";
                if (axisState.ContainsKey(gp + axis))
                {
                    return axisState[gp + axis];
                }
            }
            

            return 0f;
        }

        /// <summary>
        /// Update the input states.
        /// </summary>
        static public void update()
        {
            // Shift keyStates to keyStatesOld
            
            foreach(string key in keys)
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

            // Handle Keys
            for (int pressedKeyIndex = 0; pressedKeyIndex < pressedKeys.Length; pressedKeyIndex++)
            {
                string keyName = pressedKeys[pressedKeyIndex].ToString().ToLower();
                // Handle stupid MS key names
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
            float width = (float)GammaDraconis.GetInstance().GraphicsDevice.DisplayMode.Width;
            float height = (float)GammaDraconis.GetInstance().GraphicsDevice.DisplayMode.Height;
            float mouseX = (mouseState.X + GammaDraconis.GetInstance().Window.ClientBounds.X - (width/2)) / width;
            float mouseY = -(mouseState.Y + GammaDraconis.GetInstance().Window.ClientBounds.Y - (height/2)) / height;
            if (Math.Abs( mouseX ) < 0.02 )
            {
                axisState["MouseX"] = 0.0f;
            } else {
                axisState["MouseX"] = mouseX;
            }
            if (Math.Abs( mouseY ) < 0.02 )
            {
                axisState["MouseY"] = 0.0f;
            } else {
                axisState["MouseY"] = mouseY;
            }
                buttonState("Mouse1", mouseState.LeftButton);
            buttonState("Mouse2", mouseState.RightButton);
            buttonState("Mouse3", mouseState.MiddleButton);

            #region GamePad state handling
            for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
            {
                string gp = "Pad" + (int)index + "-";

                if (GamePad.GetCapabilities(index).IsConnected
                    && GamePad.GetCapabilities(index).GamePadType == GamePadType.GamePad)
                {
                    GamePadState gamepadState = GamePad.GetState(index);

                    buttonState(gp + "PadUp", gamepadState.DPad.Up);
                    buttonState(gp + "PadDown", gamepadState.DPad.Down);
                    buttonState(gp + "PadLeft", gamepadState.DPad.Left);
                    buttonState(gp + "PadRight", gamepadState.DPad.Right);

                    buttonState(gp + "PadA", gamepadState.Buttons.A);
                    buttonState(gp + "PadB", gamepadState.Buttons.B);
                    buttonState(gp + "PadX", gamepadState.Buttons.X);
                    buttonState(gp + "PadY", gamepadState.Buttons.Y);
                    buttonState(gp + "PadLB", gamepadState.Buttons.LeftShoulder);
                    buttonState(gp + "PadRB", gamepadState.Buttons.RightShoulder);
                    buttonState(gp + "PadLS", gamepadState.Buttons.LeftStick);
                    buttonState(gp + "PadRS", gamepadState.Buttons.RightStick);
                    buttonState(gp + "PadBack", gamepadState.Buttons.Back);
                    buttonState(gp + "PadStart", gamepadState.Buttons.Start);

                    axisState[gp + "LeftX"] = gamepadState.ThumbSticks.Left.X;
                    axisState[gp + "LeftY"] = gamepadState.ThumbSticks.Left.Y;
                    axisState[gp + "RightX"] = gamepadState.ThumbSticks.Right.X;
                    axisState[gp + "RightY"] = gamepadState.ThumbSticks.Right.Y;
                    axisState[gp + "LeftT"] = gamepadState.Triggers.Left;
                    axisState[gp + "RightT"] = gamepadState.Triggers.Right;
                    axisState[gp + "Triggers"] = (gamepadState.Triggers.Right - gamepadState.Triggers.Left);
                }
            }
            #endregion

            // Find new key presses
            foreach (string key in keys)
            {
                
                // keypressed are only registered while the key remains down, and either was not down last pass, 
                // or was registered as pressed but not checked yet.
                keyPresses[key] = ((keyStates[key] == KeyState.Down) 
                               && ((keyStatesOld[key] == KeyState.Up) || keyPresses[key]));
            }
        }

        /// <summary>
        /// Reset keyStates and keyPresses.
        /// </summary>
        static public void reset()
        {
            Input.inputReset = true;

            // Reset input dictionaries
            keys = new List<string>();
            keyStates = new Dictionary<string, KeyState>();
            keyStatesOld = new Dictionary<string, KeyState>();
            keyPresses = new Dictionary<string, bool>();
            axisState = new Dictionary<string, float>();
            
            // Loop through all useful keyboard keys
            for (char c = '!'; c <= '~'; c++)
            {
                string key = "" + c;
                keys.Add(key);
                keys.Add("control+" + key);
                keys.Add("control+alt+" + key);
                keys.Add("control+alt+shift" + key);
                keys.Add("control+shift+" + key);
                keys.Add("alt+" + key);
                keys.Add("alt+shift+" + key);
                keys.Add("shift+" + key);
            }

            // Non-alphanumeric keys
            keys.Add("control");
            keys.Add("shift");
            keys.Add("alt");
            keys.Add("up");
            keys.Add("down");
            keys.Add("left");
            keys.Add("right");
            keys.Add("enter");
            keys.Add("space");
            keys.Add("escape");
            keys.Add("comma");
            keys.Add("period");
            keys.Add("backspace");
            keys.Add("decimal");

            keys.Add("numpad1");
            keys.Add("numpad2");
            keys.Add("numpad3");
            keys.Add("numpad4");
            keys.Add("numpad5");
            keys.Add("numpad6");
            keys.Add("numpad7");
            keys.Add("numpad8");
            keys.Add("numpad9");
            keys.Add("numpad0");

            axisState.Add("MouseX", 0f);
            axisState.Add("MouseY", 0f);
            // Gamepad keys
            for(PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
            {
                string gp = "Pad" + (int)index + "-";
                keys.Add(gp + "PadUp");
                keys.Add(gp + "PadDown");
                keys.Add(gp + "PadLeft");
                keys.Add(gp + "PadRight");

                keys.Add(gp + "PadA");
                keys.Add(gp + "PadB");
                keys.Add(gp + "PadX");
                keys.Add(gp + "PadY");
                keys.Add(gp + "PadLB");
                keys.Add(gp + "PadRB");
                keys.Add(gp + "PadLS");
                keys.Add(gp + "PadRS");
                keys.Add(gp + "PadBack");
                keys.Add(gp + "PadStart");

                // Gamepad handling
                axisState.Add(gp + "LeftX", 0f);
                axisState.Add(gp + "LeftY", 0f);
                axisState.Add(gp + "RightX", 0f);
                axisState.Add(gp + "RightY", 0f);
                axisState.Add(gp + "LeftT", 0f);
                axisState.Add(gp + "RightT", 0f);
                axisState.Add(gp + "Triggers", 0f);
            }

            // Reset key states and presses
            foreach (string key in keys)
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
        public string getKeyBinding(string action)
        {
            return inputKeys[action];
        }

        /// <summary>
        /// Given a button name and state, set the appropriate key state.
        /// </summary>
        /// <param name="buttonName">Button name</param>
        /// <param name="buttonState">Button state</param>
        static private void buttonState(string buttonName, ButtonState buttonState)
        {
            if (keyStates.ContainsKey(buttonName))
            {
                keyStates[buttonName] = (buttonState == ButtonState.Pressed ? KeyState.Down : KeyState.Up);
            }
        }
    }
}
