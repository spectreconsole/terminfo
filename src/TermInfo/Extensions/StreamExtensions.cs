using System;
using System.IO;
using System.Text;

namespace TermInfo
{
    internal static class StreamExtensions
    {
        public static string? ReadString(this Stream stream, int count)
        {
            var buffer = new byte[count];
            var read = stream.Read(buffer, 0, count);
            if (read != count)
            {
                return null;
            }

            return Encoding.ASCII.GetString(buffer).TrimEnd('\0');
        }

        public static short ReadShort(this Stream stream)
        {
            var first = stream.ReadByte();
            var second = stream.ReadByte();

            if (first < 0 || second < 0)
            {
                throw new InvalidOperationException("Encountered illegal short integer value");
            }

            return (short)((256 * second) + first);
        }
    }
}
