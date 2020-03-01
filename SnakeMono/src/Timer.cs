﻿using Microsoft.Xna.Framework;

 namespace SnakeMono
{
    public class Timer
    {
        private int _time = 0;
        private int _duration;

        public Timer()
        {
            _duration = Constants.CountDuration;
        }

        public Timer(int duration)
        {
            _duration = duration;
        }

        public bool IsTimeUp()
        {
            if (_time < _duration)
                return false;
            _time -= _duration;
            return true;
        }

        public void Update(GameTime time)
        {
            _time += time.ElapsedGameTime.Milliseconds;
        }
    }
}