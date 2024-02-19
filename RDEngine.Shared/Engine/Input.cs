using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RDEngine.Engine
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    public enum KeyGate
    {
        Down,
        Held,
        Up
    }

    public class Input
    {
        private MouseState _mouse;
        private MouseState _lastMouse;
        private KeyboardState _keyboard;
        private KeyboardState _lastKeyboard;

        public static Input Instance;

        private Dictionary<string, Keys[]> _macros = new Dictionary<string, Keys[]>()
        {
            { "Left", new Keys[]{ Keys.Left, Keys.A} },
            { "Right", new Keys[]{ Keys.Right, Keys.D} },
            { "Up", new Keys[] {Keys.Up, Keys.W} },
            { "Down", new Keys[] {Keys.Down, Keys.S} },
            { "Jump", new Keys[]{ Keys.Up, Keys.W, Keys.Space, Keys.C} }
        };

        public Vector2 MousePos
        {
            get
            {
                return (_mouse.Position.ToVector2() - SceneHandler.ActiveScene.CameraPos) / RDEGame.ScaleFactor;
            }
        }
        public Vector2 RealMousePos
        {
            get
            {
                return _mouse.Position.ToVector2() - SceneHandler.ActiveScene.CameraPos;
            }
        }

        public void UpdateInput()
        {
            _mouse = Mouse.GetState();
            _keyboard = Keyboard.GetState();
        }
        public void UpdateLastInput()
        {
            _lastMouse = _mouse;
            _lastKeyboard = _keyboard;
        }

        public bool GetButton(MouseButton button, KeyGate state)
        {
            bool pressed;
            bool wasPressed;

            switch (button)
            {
                case MouseButton.Left:
                    pressed = _mouse.LeftButton == ButtonState.Pressed;
                    wasPressed = _lastMouse.LeftButton == ButtonState.Pressed;
                    break;
                case MouseButton.Right:
                    pressed = _mouse.RightButton == ButtonState.Pressed;
                    wasPressed = _lastMouse.RightButton == ButtonState.Pressed;
                    break;
                case MouseButton.Middle:
                    pressed = _mouse.MiddleButton == ButtonState.Pressed;
                    wasPressed = _lastMouse.MiddleButton == ButtonState.Pressed;
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }

            return Evaluate(pressed, wasPressed, state);
        }
        public bool GetKey(Keys key, KeyGate state)
        {
            bool pressed = _keyboard.IsKeyDown(key);
            bool wasPressed = _lastKeyboard.IsKeyDown(key);

            return Evaluate(pressed, wasPressed, state);
        }

        public bool GetMacro(string name, KeyGate state)
        {
            if (!_macros.ContainsKey(name))
                throw new KeyNotFoundException($"Macro '{name}' doesn't exist");

            bool pressed = _keyboard.GetPressedKeys().Intersect(_macros[name]).Count() > 0;
            bool wasPressed = _lastKeyboard.GetPressedKeys().Intersect(_macros[name]).Count() > 0;

            return Evaluate(pressed, wasPressed, state);
        }

        private bool Evaluate(bool pressed, bool wasPressed, KeyGate state)
        {
            if (pressed && !wasPressed && state == KeyGate.Down)
                return true;
            else if (pressed && state == KeyGate.Held)
                return true;
            else if (!pressed && wasPressed && state == KeyGate.Up)
                return true;
            return false;
        }
    }
}