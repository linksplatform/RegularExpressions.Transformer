using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public static class ITextTransformersListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<string> TransformWithAll(this IList<ITextTransformer> transformers, string source)
        {
            var strings = new List<string>();
            if (transformers.Count > 0)
            {
                for (int i = 0; i < transformers.Count; i++)
                {
                    strings.Add(transformers[i].Transform(source));
                }
            }
            return strings;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformWithAllToFiles(this IList<ITextTransformer> transformers, string sourceText, string targetPath, bool skipFilesWithNoChanges)
        {
            if (transformers.Count > 0)
            {
                var directoryName = Path.GetDirectoryName(targetPath);
                var targetFilename = Path.GetFileNameWithoutExtension(targetPath);
                var targetExtension = Path.GetExtension(targetPath);
                var lastText = "";
                for (int i = 0; i < transformers.Count; i++)
                {
                    var transformationOutput = transformers[i].Transform(sourceText);
                    if (!(skipFilesWithNoChanges && string.Equals(lastText, transformationOutput)))
                    {
                        lastText = transformationOutput;
                        File.WriteAllText(Path.Combine(directoryName, $"{targetFilename}.{i}{targetExtension}"), transformationOutput, Encoding.UTF8);
                    }
                }
            }
        }
    }
}
