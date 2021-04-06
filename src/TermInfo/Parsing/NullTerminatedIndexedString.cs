using System;

namespace TermInfo
{
    internal sealed class NullTerminatedIndexedString
    {
        private ReadOnlyMemory<char> _data;
        private ReadOnlyMemory<int> _indexes;

        public NullTerminatedIndexedString(string data, int[] indexes)
        {
            _data = data.AsMemory();
            _indexes = indexes.AsMemory();
        }

        public string[] GetStrings(int count, bool sliceData = false)
        {
            var result = new string[count];

            var last = 0;
            for (var i = 0; i < count; i++)
            {
                var start = _indexes.Span[i];
                if (start < 0)
                {
                    continue;
                }

                var end = FindNull(start);
                if (end != -1)
                {
                    result[i] = _data.Slice(start, end - start).ToString();
                    last = end + 1;
                }
                else
                {
                    throw new InvalidOperationException("Invalid string table!");
                }
            }

            _indexes = _indexes.Slice(count);

            if (sliceData)
            {
                _data = _data.Slice(last);
            }

            return result;
        }

        private int FindNull(int start)
        {
            for (var i = start; i < _data.Length; i++)
            {
                if (_data.Span[i] == '\0')
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
