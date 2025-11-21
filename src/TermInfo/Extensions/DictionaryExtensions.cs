namespace TermInfo;

internal static class DictionaryExtensions
{
    public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TElement> valueSelector)
        where TKey : notnull
    {
        var dictionary = new Dictionary<TKey, TElement>();
        foreach (var item in source)
        {
            var key = keySelector(item);
            var value = valueSelector(item);

            dictionary[key] = value;
        }

        return dictionary;
    }
}