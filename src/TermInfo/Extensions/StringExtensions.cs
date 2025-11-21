namespace TermInfo;

using System.Text;

internal static class StringExtensions
{
    public static string ReadNullTerminatedString(this string source, int offset)
    {
        var accumulator = new StringBuilder();
        for (var pos = offset; pos < source.Length; pos++)
        {
            if (source[pos] == '\0')
            {
                break;
            }

            accumulator.Append(source[pos]);
        }

        return accumulator.ToString();
    }
}