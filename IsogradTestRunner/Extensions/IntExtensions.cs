using System;

namespace IsogradTestRunner.Extensions
{
    public static class IntExtensions
    {
        public static TimeSpan Seconds(this int value) => TimeSpan.FromSeconds(value);
        public static TimeSpan Milliseconds(this int value) => TimeSpan.FromMilliseconds(value);
    }
}