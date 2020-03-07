using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    internal static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetPathParts(this string path, out string directoryName, out string targetFilename, out string targetExtension) => (directoryName, targetFilename, targetExtension) = (Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path), Path.GetExtension(path));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteToFile(this string text, string directoryName, string targetFilename) => File.WriteAllText(Path.Combine(directoryName, targetFilename), text, Encoding.UTF8);
    }
}
