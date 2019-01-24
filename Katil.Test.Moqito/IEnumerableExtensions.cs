using System.Collections.Generic;
using System.Linq;

namespace Katil.Test.Moqito
{
    public static class IEnumerableExtensions
    {
        public static IQueryable<T> TestAsync<T>(this IEnumerable<T> source)
        {
            return new TestAsyncEnumerable<T>(source);
        }
    }
}
