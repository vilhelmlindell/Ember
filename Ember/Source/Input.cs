using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Ember
{
    public enum MouseButton
    {
        Left,
        Middle,
        Right,
        Forward,
        Back
    }

    public static class Input
    {
        private static KeyboardState _keyState;
        private static KeyboardState _previousKeyState;
        private static MouseState _mouseState;
        private static MouseState _previousMouseState;
        private static Dictionary<MouseButton, ButtonState> _mouseButtonStates;
        private static Dictionary<MouseButton, ButtonState> _previousMouseButtonStates;

        static Input()
        {
            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();

            _mouseButtonStates = new Dictionary<MouseButton, ButtonState>()
            {
                { MouseButton.Left, _mouseState.LeftButton },
                { MouseButton.Middle, _mouseState.MiddleButton },
                { MouseButton.Right, _mouseState.RightButton },
                { MouseButton.Forward, _mouseState.XButton2 },
                { MouseButton.Back, _mouseState.XButton1 }
            };

            _previousMouseButtonStates = new Dictionary<MouseButton, ButtonState>()
            {
                { MouseButton.Left, _previousMouseState.LeftButton },
                { MouseButton.Middle, _previousMouseState.MiddleButton },
                { MouseButton.Right, _previousMouseState.RightButton },
                { MouseButton.Forward, _previousMouseState.XButton2 },
                { MouseButton.Back, _previousMouseState.XButton1 }
            };
        }

        public static Vector2 MousePosition => new(_mouseState.X, _mouseState.Y);
        public static Vector2 PreviousMousePosition => new(_previousMouseState.X, _previousMouseState.Y);
        public static int ScrollWheelValue => _mouseState.ScrollWheelValue;
        public static int PreviousScrollWheelValue => _previousMouseState.ScrollWheelValue;

        public static void Update()
        {
            _previousKeyState = _keyState;
            _previousMouseState = _mouseState;
            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();

            _mouseButtonStates[MouseButton.Left] = _mouseState.LeftButton;
            _mouseButtonStates[MouseButton.Middle] = _mouseState.MiddleButton;
            _mouseButtonStates[MouseButton.Right] = _mouseState.RightButton;
            _mouseButtonStates[MouseButton.Forward] = _mouseState.XButton2;
            _mouseButtonStates[MouseButton.Back] = _mouseState.XButton1;

            _previousMouseButtonStates[MouseButton.Left] = _previousMouseState.LeftButton;
            _previousMouseButtonStates[MouseButton.Middle] = _previousMouseState.MiddleButton;
            _previousMouseButtonStates[MouseButton.Right] = _previousMouseState.RightButton;
            _previousMouseButtonStates[MouseButton.Forward] = _previousMouseState.XButton2;
            _previousMouseButtonStates[MouseButton.Back] = _previousMouseState.XButton1;
        }

        public static bool KeyDown(Keys key)
        {
            return _keyState.IsKeyDown(key);
        }
        public static bool KeyUp(Keys key)
        {
            return _keyState.IsKeyUp(key);
        }
        public static bool KeyPressed(Keys key)
        {
            return _previousKeyState.IsKeyUp(key) && _keyState.IsKeyDown(key);
        }
        public static bool KeyReleased(Keys key)
        {
            return _previousKeyState.IsKeyDown(key) && _keyState.IsKeyUp(key);
        }
        public static bool MouseDown(MouseButton button)
        {
            return _mouseButtonStates[button] == ButtonState.Pressed;
        }
        public static bool MouseUp(MouseButton button)
        {
            return _mouseButtonStates[button] == ButtonState.Released;
        }
        public static bool MousePressed(MouseButton button)
        {
            return _previousMouseButtonStates[button] == ButtonState.Released &&
                   _mouseButtonStates[button] == ButtonState.Pressed;
        }
        public static bool MouseReleased(MouseButton button)
        {
            return _previousMouseButtonStates[button] == ButtonState.Pressed &&
                   _mouseButtonStates[button] == ButtonState.Released;
        }
    }
}
