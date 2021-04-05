using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace TermInfo
{
    /// <summary>
    /// Represents a parsed terminfo description.
    /// </summary>
    public sealed partial class TermInfoData
    {
        private readonly string[] _names;
        private readonly bool[] _booleans;
        private readonly int[] _nums;
        private readonly string?[] _strings;

        /// <summary>
        /// Gets the names of the parsed terminfo description.
        /// </summary>
        public string[] Names => _names;

        internal TermInfoData(string[] names, bool[] booleans, int[] nums, string?[] strings)
        {
            _names = names;
            _booleans = booleans;
            _nums = nums;
            _strings = strings;
        }

        /// <summary>
        /// Reads terminfo description from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed terminfo description.</returns>
        public static TermInfoData Read(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return Parser.Parse(stream);
        }

        /// <summary>
        /// Gets a specific boolean terminfo capability value.
        /// </summary>
        /// <param name="value">The capability to get the value for.</param>
        /// <returns>The terminfo capability value.</returns>
        public bool? GetBoolean(TermInfoCaps.Boolean value)
        {
            if (TryGetBoolean(value, out var result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets a specific numeric terminfo capability value.
        /// </summary>
        /// <param name="value">The capability to get the value for.</param>
        /// <returns>The terminfo capability value.</returns>
        public int? GetNum(TermInfoCaps.Num value)
        {
            if (TryGetNum(value, out var result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets a specific string terminfo capability value.
        /// </summary>
        /// <param name="value">The capability to get the value for.</param>
        /// <returns>The terminfo capability value.</returns>
        public string? GetString(TermInfoCaps.String value)
        {
            if (TryGetString(value, out var result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Tries to get a specific boolean terminfo capability value.
        /// </summary>
        /// <param name="value">The capability to get the value for.</param>
        /// <param name="result">The terminfo capability value, or <c>null</c> if missing.</param>
        /// <returns><c>true</c> if the capability was found, otherwise <c>false</c>.</returns>
        public bool TryGetBoolean(TermInfoCaps.Boolean value, [NotNullWhen(true)] out bool? result)
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

        /// <summary>
        /// Tries to get a specific numeric terminfo capability value.
        /// </summary>
        /// <param name="value">The capability to get the value for.</param>
        /// <param name="result">The terminfo capability value, or <c>null</c> if missing.</param>
        /// <returns><c>true</c> if the capability was found, otherwise <c>false</c>.</returns>
        public bool TryGetNum(TermInfoCaps.Num value, [NotNullWhen(true)] out int? result)
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

        /// <summary>
        /// Tries to get a specific string terminfo capability value.
        /// </summary>
        /// <param name="value">The capability to get the value for.</param>
        /// <param name="result">The terminfo capability value, or <c>null</c> if missing.</param>
        /// <returns><c>true</c> if the capability was found, otherwise <c>false</c>.</returns>
        public bool TryGetString(TermInfoCaps.String value, [NotNullWhen(true)] out string? result)
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
