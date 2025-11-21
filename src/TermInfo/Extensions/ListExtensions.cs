namespace TermInfo;

using System.Collections.Generic;

internal static class ListExtensions
{
    public static void AddIfNotNullOrEmpty(this IList<string> source, string? item)
    {
        if (!string.IsNullOrWhiteSpace(item))
        {
            source.Add(item);
        }
    }
}