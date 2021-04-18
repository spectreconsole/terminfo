using System;
using System.IO;

namespace TermInfo
{
    /// <summary>
    /// Represents a parsed terminfo description.
    /// </summary>
    public sealed partial class TermInfoDesc
    {
        /// <summary>
        /// Tries to load the default terminfo description for the current terminal.
        /// </summary>
        /// <param name="result">
        /// When this method returns, contains the terminfo description,
        /// if the loading succeeded, or <c>null</c> if the loading  failed.
        /// </param>
        /// <returns><c>true</c> if the terminfo description was loaded successfully; otherwise, <c>false</c>.</returns>
        public static bool TryLoad(out TermInfoDesc? result)
        {
            try
            {
                result = Load();
                return result != null;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Loads the default terminfo description for the current terminal.
        /// </summary>
        /// <returns>The default terminfo description for the current terminal,
        /// or <c>null</c> if none could be resolved.</returns>
        public static TermInfoDesc? Load()
        {
            return TermInfoLoader.Load();
        }

        /// <summary>
        /// Tries to load the specified terminfo description for the current terminal.
        /// </summary>
        /// <param name="name">The terminfo name to load.</param>
        /// <param name="result">
        /// When this method returns, contains the terminfo description
        /// if the loading succeeded, or <c>null</c> if the loading failed.
        /// </param>
        /// <returns><c>true</c> if the terminfo description was loaded successfully; otherwise, <c>false</c>.</returns>
        public static bool TryLoad(string name, out TermInfoDesc? result)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            try
            {
                result = Load(name);
                return result != null;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Loads the specified terminfo description for the current terminal.
        /// </summary>
        /// <param name="name">The name of the terminfo description to load.</param>
        /// <returns>The default terminfo description for the current terminal,
        /// or <c>null</c> if none could be resolved.</returns>
        public static TermInfoDesc? Load(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return TermInfoLoader.Load(name);
        }

        /// <summary>
        /// Tries to read a terminfo description from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="result">
        /// When this method returns, contains the terminfo description
        /// if the loading succeeded, or <c>null</c> if the loading failed.
        /// </param>
        /// <returns><c>true</c> if the terminfo description was loaded successfully; otherwise, <c>false</c>.</returns>
        public static bool TryLoad(Stream stream, out TermInfoDesc? result)
        {
            try
            {
                result = Load(stream);
                return result != null;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Reads a terminfo description from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed terminfo description.</returns>
        public static TermInfoDesc Load(Stream stream)
        {
            return TermInfoLoader.Load(stream);
        }
    }
}
