﻿using Microsoft.Xna.Framework.Input;

 namespace SnakeMono
{
    public class Input
    {
        private KeyboardState _previousState, _state;

        public Input()
        {
            _previousState = _state = Keyboard.GetState();
        }

        public bool KeyPress(Keys key)
        {
            return _previousState.IsKeyUp(key) && _state.IsKeyDown(key);
        }

        public bool KeyDown(Keys key)
        {
            return _state.IsKeyDown(key);
        }

        public bool KeyUp(Keys key)
        {
            return _state.IsKeyUp(key);
        }

        public void Update()
        {
            _previousState = _state;
            _state = Keyboard.GetState();
        }
    }
}