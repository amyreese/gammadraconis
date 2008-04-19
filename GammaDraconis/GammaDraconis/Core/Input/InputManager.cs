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
            None, GamePad, KeyboardWASD, KeyboardNumPad
        }

        private Dictionary<PlayerIndex, ControlScheme> controlMappings;
        private PlayerIndex mouseUser;
        private bool mouseInUse;

        public InputManager()
        {
            controlMappings = new Dictionary<PlayerIndex, ControlScheme>();
            AutoRegisterControlSchemes();
            mouseInUse = false;
        }

        public void AutoRegisterControlSchemes()
        {
            if (GamePad.GetCapabilities(PlayerIndex.One).IsConnected)
            {
                controlMappings.Add(PlayerIndex.One, ControlScheme.GamePad);
            }
            else
            {
                controlMappings.Add(PlayerIndex.One, ControlScheme.KeyboardWASD);
            }

            if (GamePad.GetCapabilities(PlayerIndex.Two).IsConnected)
            {
                controlMappings.Add(PlayerIndex.Two, ControlScheme.GamePad);
            }
            else
            {
                if (IsSchemeAvailable(ControlScheme.KeyboardWASD))
                {
                    controlMappings.Add(PlayerIndex.Two, ControlScheme.KeyboardWASD);
                }
                else
                {
                    controlMappings.Add(PlayerIndex.Two, ControlScheme.KeyboardNumPad);
                }
            }

            if (GamePad.GetCapabilities(PlayerIndex.Three).IsConnected)
            {
                controlMappings.Add(PlayerIndex.Three, ControlScheme.GamePad);
            }
            else
            {
                if (IsSchemeAvailable(ControlScheme.KeyboardWASD))
                {
                    controlMappings.Add(PlayerIndex.Three, ControlScheme.KeyboardWASD);
                }
                else
                {
                    controlMappings.Add(PlayerIndex.Three, ControlScheme.KeyboardNumPad);
                }
            }

            if (GamePad.GetCapabilities(PlayerIndex.Four).IsConnected)
            {
                controlMappings.Add(PlayerIndex.Four, ControlScheme.GamePad);
            }
            else
            {
                if (IsSchemeAvailable(ControlScheme.KeyboardWASD))
                {
                    controlMappings.Add(PlayerIndex.Four, ControlScheme.KeyboardWASD);
                }
                else
                {
                    controlMappings.Add(PlayerIndex.Four, ControlScheme.KeyboardNumPad);
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
