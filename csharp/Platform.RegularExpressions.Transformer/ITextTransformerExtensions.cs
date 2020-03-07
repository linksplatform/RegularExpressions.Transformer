using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Platform.Collections;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public static class ITextTransformerExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<ITextTransformer> GenerateTransformersForEachRule(this ITextTransformer transformer)
        {
            var transformers = new List<ITextTransformer>();
            for (int i = 1; i <= transformer.Rules.Count; i++)
            {
                transformers.Add(new TextTransformer(transformer.Rules.Take(i).ToList()));
            }
            return transformers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<string> GetSteps(this ITextTransformer transformer, string sourceText)
        {
            if (transformer != null && !transformer.Rules.IsNullOrEmpty())
            {
                var steps = new List<string>();
                var steppedTransformer = new TextSteppedTransformer(transformer.Rules, sourceText);
                while (steppedTransformer.Next())
                {
                    steps.Add(steppedTransformer.Text);
                }
                return steps;
            }
            else
            {
                return Array.Empty<string>();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStepsToFiles(this ITextTransformer transformer, string sourceText, string targetPath, bool skipFilesWithNoChanges)
        {
            if(transformer != null && !transformer.Rules.IsNullOrEmpty())
            {
                targetPath.GetPathParts(out var directoryName, out var targetFilename, out var targetExtension);
                var lastText = "";
                var steppedTransformer = new TextSteppedTransformer(transformer.Rules, sourceText);
                while (steppedTransformer.Next())
                {
                    var newText = steppedTransformer.Text;
                    steppedTransformer.WriteStep(directoryName, targetFilename, targetExtension, steppedTransformer.Current, ref lastText, newText, skipFilesWithNoChanges);
                }
            }
        }
    }
}
