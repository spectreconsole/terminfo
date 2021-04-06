using System.IO;

namespace TermInfo
{
    /// <summary>
    /// Represents a parsed terminfo description.
    /// </summary>
    public sealed partial class TermInfoDesc
    {
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
        /// Loads the specified terminfo description for the current terminal.
        /// </summary>
        /// <param name="name">The name of the terminfo description to load.</param>
        /// <returns>The default terminfo description for the current terminal,
        /// or <c>null</c> if none could be resolved.</returns>
        public static TermInfoDesc? Load(string name)
        {
            if (name is null)
            {
                throw new System.ArgumentNullException(nameof(name));
            }

            return TermInfoLoader.Load(name);
        }

        /// <summary>
        /// Reads terminfo description from a stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The parsed terminfo description.</returns>
        public static TermInfoDesc Load(Stream stream)
        {
            return TermInfoLoader.Load(stream);
        }
    }
}
