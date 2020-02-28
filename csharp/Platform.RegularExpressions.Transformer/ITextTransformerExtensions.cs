using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
        public static List<string> GetSteps(this ITextTransformer transformer, string sourceText) => transformer.GenerateTransformersForEachRule().TransformWithAll(sourceText);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStepsToFiles(this ITextTransformer transformer, string sourceText, string targetPath, bool skipFilesWithNoChanges) => transformer.GenerateTransformersForEachRule().TransformWithAllToFiles(sourceText, targetPath, skipFilesWithNoChanges);
    }
}
