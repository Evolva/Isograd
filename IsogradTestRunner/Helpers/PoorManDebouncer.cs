using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace IsogradTestRunner.Helpers
{
    public class PoorManDebouncer<T, TProjection>
        where TProjection : IEquatable<TProjection>
    {
        private readonly Action<T> _actionToDebounce;
        private readonly Func<T, TProjection> _debounceBy;
        private readonly TimeSpan _delay;
        private readonly IDictionary<TProjection, Timer> _timerByProjectionValue;

        public PoorManDebouncer(Action<T> actionToDebounce, Func<T, TProjection> debounceBy, TimeSpan delay)
        {
            _actionToDebounce = actionToDebounce;
            _debounceBy = debounceBy;
            _delay = delay;
            _timerByProjectionValue = new ConcurrentDictionary<TProjection, Timer>();
        }

        public void DebouncedActionFor(T value)
        {
            Timer timer;
            var projectionValue = _debounceBy(value);
            if (!_timerByProjectionValue.TryGetValue(projectionValue, out timer))
            {
                timer = new Timer
                {
                    AutoReset = false,
                    Enabled = true,
                    Interval = _delay.TotalMilliseconds
                };
                timer.Elapsed += (_, __) => _actionToDebounce(value);
                _timerByProjectionValue.Add(projectionValue, timer);
            }

            timer.Stop();
            timer.Start();
        }
    }
}