using System.Collections.Generic;

namespace Minigram.Api.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> From<T>(params T[] elements)
        {
            return elements;
        }
    }
}