using System;
using Microsoft.Xna.Framework;
using Ember.Utilities;

namespace Ember.Easing
{
    public class Easer : IEaser
    {
        private Func<float, float> _function;
        private float _durationInSeconds;
        private float _elapsedSeconds;
        private float _repeatDelay;
        private bool _beginCalled;
        private bool _isPaused;
        private bool _repeats;
        private bool _reverses;

        public Action EaseFinished;

        public void Update(GameTime gameTime)
        {
            if (_beginCalled && !_isPaused)
            {
                if (_reverses)
                {
                    _elapsedSeconds -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _elapsedSeconds = Math.Max(_elapsedSeconds, _durationInSeconds);
                }
                else
                {
                    _elapsedSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _elapsedSeconds = Math.Min(_elapsedSeconds, _durationInSeconds);
                }

                if (_elapsedSeconds == _durationInSeconds)
                {
                    _elapsedSeconds = 0;
                }
            }
        }
    }
}
