using System;
using System.Collections.Generic;

namespace Covid19Api.Repositories.Extensions
{
    internal static class ListExtensions
    {
        public static IEnumerable<List<T>> CreateChunks<T>(this List<T> elementsOfTypeT, int chunkSize)
        {
            for (var i = 0; i < elementsOfTypeT.Count; i += chunkSize)
                yield return elementsOfTypeT.GetRange(i, Math.Min(chunkSize, elementsOfTypeT.Count - i));
        }
    }
}