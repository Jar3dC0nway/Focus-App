using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus_App.Scripts.Foundation.Utility
{
    public static class Input
    {
        static KeyInput[] keys = new KeyInput[0];
        static MouseInput[] mouse = new MouseInput[3];
        public static KeyInput[] GetInput() {
            return keys;
        }
        public static bool IsKeyPressed(Keys key) {
            for (int i = 0; i < keys.Length; i++) { 
                if (keys[i].Key == key)
                    return true;
            }
            return false;
        }
        public static bool IsKeyJustPressed(Keys key)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].Key == key)
                    return keys[i].justPressed;
            }
            return false;
        }
        public static bool IsMouseClicked(MouseKeys key) {
            for (int i = 0; i < mouse.Length; i++)
            {
                if (mouse[i] != null && mouse[i].Key == key)
                    return true;
            }
            return false;
        }
        public static bool IsMouseJustClicked(MouseKeys key)
        {
            for (int i = 0; i < mouse.Length; i++)
            {
                if (mouse[i] != null && mouse[i].Key == key)
                    return mouse[i].justPressed;
            }
            return false;
        }


        private static void _UpdateKeyInput()
        {
            KeyboardState pressed = Keyboard.GetState();
            KeyInput[] newKeys = new KeyInput[pressed.GetPressedKeyCount()];
            for (int i = 0; i < newKeys.Length; i++)
            {
                KeyInput newKey = new KeyInput();
                newKey.Key = pressed.GetPressedKeys()[i];
                newKey.justPressed = true;
                foreach (KeyInput keystate in keys)
                {
                    if (keystate.Key == newKey.Key)
                    {
                        newKey.justPressed = false;
                        break;
                    }
                }
                newKeys[i] = newKey;
            }
            keys = newKeys;
        }

        private static MouseInput _SetMouseKey(MouseKeys key) {
            return new MouseInput().K(key).B(mouse[(int)key] == null || mouse[(int)key].Key != key);
        }

        private static void _UpdateMouseInput() {
            MouseState pressed = Mouse.GetState();
            MouseInput[] newKeys = new MouseInput[3];

            newKeys[0] = (pressed.LeftButton == ButtonState.Pressed) ? _SetMouseKey(MouseKeys.Left) : null;
            newKeys[1] = (pressed.RightButton == ButtonState.Pressed) ? _SetMouseKey(MouseKeys.Right) : null;
            newKeys[2] = (pressed.MiddleButton == ButtonState.Pressed) ? _SetMouseKey(MouseKeys.Middle) : null;

            mouse = newKeys;
        }

        public static void UpdateInput() {
            _UpdateKeyInput();
            _UpdateMouseInput();
        }
    }
}
