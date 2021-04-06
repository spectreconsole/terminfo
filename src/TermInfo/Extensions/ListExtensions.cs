using System.Collections.Generic;

namespace TermInfo
{
    internal static class ListExtensions
    {
        public static void AddIfNotNullOrEmpty(this IList<string> source, string? item)
        {
            if (source != null && !string.IsNullOrWhiteSpace(item))
            {
                source.Add(item);
            }
        }
    }
}
