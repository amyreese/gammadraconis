using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GammaDraconis.Core.Input
{
    class InputManager
    {
        public enum ControlScheme
        {
            None, GamePad, KeyboardWASD, KeyboardNumPad, KeyboardNumPad2, KeyboardNumPad3
        }

        private Dictionary<PlayerIndex, ControlScheme> controlMappings;
        private PlayerIndex mouseUser;
        private bool mouseInUse;

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

        private void ResetControlSchemes()
        {
            controlMappings[PlayerIndex.One] = ControlScheme.None;
            controlMappings[PlayerIndex.Two] = ControlScheme.None;
            controlMappings[PlayerIndex.Three] = ControlScheme.None;
            controlMappings[PlayerIndex.Four] = ControlScheme.None;
        }

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
                    controlMappings[PlayerIndex.Three] = ControlScheme.KeyboardNumPad2;
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
                else if (IsSchemeAvailable(ControlScheme.KeyboardNumPad2))
                {
                    controlMappings[PlayerIndex.Four] = ControlScheme.KeyboardNumPad2;
                }
                else
                {
                    controlMappings[PlayerIndex.Four] = ControlScheme.KeyboardNumPad3;
                }
            }
        }

        public bool IsSchemeAvailable(ControlScheme c)
        {
            foreach (KeyValuePair<PlayerIndex, ControlScheme> kvp in controlMappings)
                if (kvp.Value == c)
                    return false;

            return true;
        }

        public bool IsMouseAvailable()
        {
            return !mouseInUse;
        }

        public PlayerInput GetPlayerInput(PlayerIndex p)
        {
            return new PlayerInput(p, controlMappings[p]);
        }
    }
}
