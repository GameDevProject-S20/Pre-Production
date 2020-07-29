namespace Extentions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtensions
    {
        /// <summary>
        /// Enable ForEach to be used on non-list IEnumerables
        /// 
        /// Warning -- may introduce side-effects
        /// </summary>
        /// <typeparam name="T">Enumerable type</typeparam>
        /// <param name="enumerable">Enumerable</param>
        /// <param name="action">ForEach action</param>
        /// <returns>The enumerable</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T elem in enumerable)
            {
                action(elem);
            }

            return enumerable;
        }
    }

    public static class StringExtentions
    {
        public static string Capitalize(this string s)
        {
            return s.First().ToString().ToUpper() + s.Substring(1).ToLower();
        }

        public static string ToTitleCase(this string s)
        {
            return string.Join(" ", s.Split(' ').Select(ss => ss.Capitalize()));
        }
    }
}
