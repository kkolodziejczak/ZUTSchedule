using System.Collections.Generic;

namespace ZUTSchedule.core
{
    public static class CollectionsExtensions
    {

        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }

        public static bool IsEmptyOrNull<T>(this ICollection<T> collection)
        {
            return collection == null || collection.IsEmpty();
        }

        public static bool IsNotEmpty<T>(this ICollection<T> collection)
        {
            return !collection.IsEmpty();
        }

        public static bool IsNotEmptyOrNull<T>(this ICollection<T> collection)
        {
            return !collection.IsEmptyOrNull();
        }
    }
}
