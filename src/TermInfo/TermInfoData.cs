using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermInfo
{
    public sealed partial class TermInfoData
    {
        private readonly string[] _names;
        private readonly bool[] _booleans;
        private readonly int[] _nums;
        private readonly string[] _strings;

        public string[] Names => _names;

        internal TermInfoData(string[] names, bool[] booleans, int[] nums, string[] strings)
        {
            _names = names;
            _booleans = booleans;
            _nums = nums;
            _strings = strings;
        }

        public static TermInfoData Read(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return Parser.Parse(stream);
        }

        public bool? GetBoolean(TermInfoCaps.Boolean value)
        {
            if (TryGetBoolean(value, out var result))
            {
                return result;
            }

            return null;
        }


        public int? GetNum(TermInfoCaps.Num value)
        {
            if (TryGetNum(value, out var result))
            {
                return result;
            }

            return null;
        }

        public string GetString(TermInfoCaps.String value)
        {
            if (TryGetString(value, out var result))
            {
                return result;
            }

            return null;
        }

        public bool TryGetBoolean(TermInfoCaps.Boolean value, out bool? result)
        {
            var index = (int)value;
            if (index >= _booleans.Length)
            {
                result = null;
                return false;
            }

            result = _booleans[index];
            return true;
        }

        public bool TryGetNum(TermInfoCaps.Num value, out int? result)
        {
            var index = (int)value;
            if (index >= _nums.Length)
            {
                result = null;
                return false;
            }

            result = _nums[index];
            if (result == -1)
            {
                result = null;
                return false;
            }

            return true;
        }

        public bool TryGetString(TermInfoCaps.String value, out string result)
        {
            var index = (int)value;
            if (index >= _strings.Length)
            {
                result = null;
                return false;
            }

            result = _strings[index];
            if (result == null)
            {
                result = null;
                return false;
            }

            return true;
        }
    }
}
