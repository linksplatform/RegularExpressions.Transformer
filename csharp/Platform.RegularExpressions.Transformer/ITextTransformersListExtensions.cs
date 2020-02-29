using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform.Collections;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public static class ITextTransformersListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<string> TransformWithAll(this IList<ITextTransformer> transformers, string source)
        {
            if (!transformers.IsNullOrEmpty())
            {
                var steps = new List<string>();
                for (int i = 0; i < transformers.Count; i++)
                {
                    steps.Add(transformers[i].Transform(source));
                }
                return steps;
            }
            else
            {
                return Array.Empty<string>();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformWithAllToFiles(this IList<ITextTransformer> transformers, string sourceText, string targetPath, bool skipFilesWithNoChanges)
        {
            if (!transformers.IsNullOrEmpty())
            {
                targetPath.GetPathParts(out var directoryName, out var targetFilename, out var targetExtension);
                var lastText = "";
                for (int i = 0; i < transformers.Count; i++)
                {
                    var transformationOutput = transformers[i].Transform(sourceText);
                    if (!(skipFilesWithNoChanges && string.Equals(lastText, transformationOutput)))
                    {
                        lastText = transformationOutput;
                        transformationOutput.WriteStepToFile(directoryName, targetFilename, targetExtension, i);
                    }
                }
            }
        }
    }
}
