using System;
using System.IO;

namespace TermInfo
{
    internal sealed class TermInfoHeader
    {
        public int NameSectionLength { get; set; }
        public int BooleanCount { get; set; }
        public int IntegerCount { get; set; }
        public int StringOffsetCount { get; set; }
        public int StringTableLength { get; set; }

        public static bool TryRead(Stream stream, out TermInfoHeader header)
        {
            var magic = stream.ReadShort();
            if (magic != 282)
            {
                header = null;
                return false;
            }

            header = new TermInfoHeader
            {
                NameSectionLength = stream.ReadShort(),
                BooleanCount = stream.ReadShort(),
                IntegerCount = stream.ReadShort(),
                StringOffsetCount = stream.ReadShort(),
                StringTableLength = stream.ReadShort(),
            };

            return true;
        }
    }
}
