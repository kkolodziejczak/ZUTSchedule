using System.Collections.Generic;

namespace ZUTSchedule.core
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static bool IsNotNullOrEmpty<T>(this ICollection<T> collection)
        {
            return !IsNullOrEmpty(collection);
        }
    }
}
