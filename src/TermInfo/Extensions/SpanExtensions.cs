namespace TermInfo;

internal static class SpanExtensions
{
    public static int FindNullTerminator(this Span<char> data, int start)
    {
        for (var i = start; i < data.Length; i++)
        {
            if (data[i] == '\0')
            {
                return i;
            }
        }

        return -1;
    }
}