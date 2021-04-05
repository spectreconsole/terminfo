using System;
using System.IO;

namespace TermInfo
{
    internal static class Parser
    {
        public static TermInfoData Parse(Stream stream)
        {
            if (stream.Position != 0)
            {
                if (!stream.CanSeek)
                {
                    throw new InvalidOperationException("Stream has not been rewinded");
                }

                stream.Seek(0, SeekOrigin.Begin);
            }

            if (!TermInfoHeader.TryRead(stream, out var header))
            {
                throw new InvalidOperationException("Could not read terminfo header");
            }

            var names = ReadNames(stream, header);
            var booleans = ReadBooleans(stream, header);

            ReadNullByte(stream);

            var nums = ReadNums(stream, header);

            var offsets = ReadStringOffsets(stream, header);
            var strings = ReadStrings(stream, header, offsets);

            return new TermInfoData(names, booleans, nums, strings);
        }

        private static string[] ReadNames(Stream stream, TermInfoHeader header)
        {
            var names = stream.ReadString(header.NameSectionLength);
            return names.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static void ReadNullByte(Stream stream)
        {
            if (stream.ReadByte() != '\0')
            {
                throw new InvalidOperationException("Expected null byte");
            }
        }

        private static bool[] ReadBooleans(Stream stream, TermInfoHeader header)
        {
            var booleans = new bool[header.BooleanCount];
            for (var i = 0; i < header.BooleanCount; i++)
            {
                booleans[i] = stream.ReadByte() == 1;
            }

            return booleans;
        }

        private static int[] ReadNums(Stream stream, TermInfoHeader header)
        {
            var nums = new int[header.IntegerCount];
            for (var i = 0; i < header.IntegerCount; i++)
            {
                nums[i] = stream.ReadShort();
            }

            return nums;
        }

        private static int[] ReadStringOffsets(Stream stream, TermInfoHeader header)
        {
            var offsets = new int[header.StringOffsetCount];
            for (var i = 0; i < header.StringOffsetCount; i++)
            {
                offsets[i] = stream.ReadShort();
                if (offsets[i] == -2)
                {
                    offsets[i] = -1;
                }
            }

            return offsets;
        }

        private static string[] ReadStrings(Stream stream, TermInfoHeader header, int[] offsets)
        {
           // Read strings
            var strings = new string[offsets.Length];
            var table = stream.ReadString(header.StringTableLength);
            for (var i = 0; i < offsets.Length; i++)
            {
                if (offsets[i] != -1)
                {
                    strings[i] = table.ReadNullTerminatedString(offsets[i]);
                }
            }

            return strings;
        }
    }
}
