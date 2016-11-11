using System;
using System.Collections.Generic;

namespace IsogradTestRunner.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var elt in source)
            {
                action(elt);
            }
        }
    }
}