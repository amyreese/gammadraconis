using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GammaDraconis.Core.Input
{
    class InputManager
    {
        /// <summary>
        /// The types of control schemes.
        /// </summary>
        public enum ControlScheme
        {
            None, GamePad, KeyboardWASD, KeyboardNumPad
        }

        private Dictionary<PlayerIndex, ControlScheme> controlMappings;
        private PlayerIndex mouseUser;
        private bool mouseInUse;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public InputManager()
        {
            controlMappings = new Dictionary<PlayerIndex, ControlScheme>();
            controlMappings.Add(PlayerIndex.One, ControlScheme.None);
            controlMappings.Add(PlayerIndex.Two, ControlScheme.None);
            controlMappings.Add(PlayerIndex.Three, ControlScheme.None);
            controlMappings.Add(PlayerIndex.Four, ControlScheme.None);

            AutoRegisterControlSchemes();

            mouseInUse = false;
        }

        /// <summary>
        /// Sets all players back to having no control scheme.
        /// </summary>
        private void ResetControlSchemes()
        {
            controlMappings[PlayerIndex.One] = ControlScheme.None;
            controlMappings[PlayerIndex.Two] = ControlScheme.None;
            controlMappings[PlayerIndex.Three] = ControlScheme.None;
            controlMappings[PlayerIndex.Four] = ControlScheme.None;
        }

        /// <summary>
        /// Automatically register the control schemes based on what controllers
        /// are enabled.
        /// </summary>
        public void AutoRegisterControlSchemes()
        {
            ResetControlSchemes();

            if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
            {
                controlMappings[PlayerIndex.One] = ControlScheme.GamePad;
            }
            else
            {
                controlMappings[PlayerIndex.One] = ControlScheme.KeyboardWASD;
            }

            if (GamePad.GetCapabilities(PlayerIndex.Two).IsConnected)
            {
                controlMappings[PlayerIndex.Two] = ControlScheme.GamePad;
            }
            else
            {
                if (IsSchemeAvailable(ControlScheme.KeyboardWASD))
                {
                    controlMappings[PlayerIndex.Two] = ControlScheme.KeyboardWASD;
                }
                else
                {
                    controlMappings[PlayerIndex.Two] = ControlScheme.KeyboardNumPad;
                }
            }

            if (GamePad.GetCapabilities(PlayerIndex.Three).IsConnected)
            {
                controlMappings[PlayerIndex.Three] = ControlScheme.GamePad;
            }
            else
            {
                if (IsSchemeAvailable(ControlScheme.KeyboardWASD))
                {
                    controlMappings[PlayerIndex.Three] = ControlScheme.KeyboardWASD;
                }
                else if (IsSchemeAvailable(ControlScheme.KeyboardNumPad))
                {
                    controlMappings[PlayerIndex.Three] = ControlScheme.KeyboardNumPad;
                }
                else
                {
                    controlMappings[PlayerIndex.Three] = ControlScheme.None;
                }
            }

            if (GamePad.GetCapabilities(PlayerIndex.Four).IsConnected)
            {
                controlMappings[PlayerIndex.Four] = ControlScheme.GamePad;
            }
            else
            {
                if (IsSchemeAvailable(ControlScheme.KeyboardWASD))
                {
                    controlMappings[PlayerIndex.Four] = ControlScheme.KeyboardWASD;
                }
                else if (IsSchemeAvailable(ControlScheme.KeyboardNumPad))
                {
                    controlMappings[PlayerIndex.Four] = ControlScheme.KeyboardNumPad;
                }
                else
                {
                    controlMappings[PlayerIndex.Four] = ControlScheme.None;
                }
            }
        }

        /// <summary>
        /// Checks to see if a specific control scheme is already in use
        /// or if it is still available.
        /// </summary>
        /// <param name="c">The control scheme to check</param>
        /// <returns></returns>
        public bool IsSchemeAvailable(ControlScheme c)
        {
            foreach (KeyValuePair<PlayerIndex, ControlScheme> kvp in controlMappings)
                if (kvp.Value == c)
                    return false;

            return true;
        }

        /// <summary>
        /// Check to see if the mouse is in use or if it is available.
        /// </summary>
        /// <returns></returns>
        public bool IsMouseAvailable()
        {
            return !mouseInUse;
        }

        /// <summary>
        /// Returns the input for a player based on their control scheme
        /// </summary>
        /// <param name="p">The player to get input for</param>
        /// <returns></returns>
        public PlayerInput GetPlayerInput(PlayerIndex p)
        {
            return new PlayerInput(p, controlMappings[p]);
        }
    }
}
