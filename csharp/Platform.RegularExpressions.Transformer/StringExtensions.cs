using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the string extensions.
    /// </para>
    /// <para></para>
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// <para>
        /// Gets the path parts using the specified path.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="path">
        /// <para>The path.</para>
        /// <para></para>
        /// </param>
        /// <param name="directoryName">
        /// <para>The directory name.</para>
        /// <para></para>
        /// </param>
        /// <param name="targetFilename">
        /// <para>The target filename.</para>
        /// <para></para>
        /// </param>
        /// <param name="targetExtension">
        /// <para>The target extension.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetPathParts(this string path, out string directoryName, out string targetFilename, out string targetExtension) => (directoryName, targetFilename, targetExtension) = (Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path), Path.GetExtension(path));

        /// <summary>
        /// <para>
        /// Writes the to file using the specified text.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="text">
        /// <para>The text.</para>
        /// <para></para>
        /// </param>
        /// <param name="directoryName">
        /// <para>The directory name.</para>
        /// <para></para>
        /// </param>
        /// <param name="targetFilename">
        /// <para>The target filename.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteToFile(this string text, string directoryName, string targetFilename) => File.WriteAllText(Path.Combine(directoryName, targetFilename), text, Encoding.UTF8);
    }
}
