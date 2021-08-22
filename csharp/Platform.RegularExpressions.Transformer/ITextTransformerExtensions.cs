using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Platform.Collections;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the text transformer extensions.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class ITextTransformerExtensions
    {
        /// <summary>
        /// <para>
        /// Generates the transformers for each rule using the specified transformer.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="transformer">
        /// <para>The transformer.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The transformers.</para>
        /// <para></para>
        /// </returns>
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

        /// <summary>
        /// <para>
        /// Gets the steps using the specified transformer.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="transformer">
        /// <para>The transformer.</para>
        /// <para></para>
        /// </param>
        /// <param name="sourceText">
        /// <para>The source text.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>A list of string</para>
        /// <para></para>
        /// </returns>
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

        /// <summary>
        /// <para>
        /// Writes the steps to files using the specified transformer.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="transformer">
        /// <para>The transformer.</para>
        /// <para></para>
        /// </param>
        /// <param name="sourceText">
        /// <para>The source text.</para>
        /// <para></para>
        /// </param>
        /// <param name="targetPath">
        /// <para>The target path.</para>
        /// <para></para>
        /// </param>
        /// <param name="skipFilesWithNoChanges">
        /// <para>The skip files with no changes.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStepsToFiles(this ITextTransformer transformer, string sourceText, string targetPath, bool skipFilesWithNoChanges)
        {
            if (transformer != null && !transformer.Rules.IsNullOrEmpty())
            {
                targetPath.GetPathParts(out var directoryName, out var targetFilename, out var targetExtension);
                Steps.DeleteAllSteps(directoryName, targetFilename, targetExtension);
                var lastText = "";
                var steppedTransformer = new TextSteppedTransformer(transformer.Rules, sourceText);
                while (steppedTransformer.Next())
                {
                    var newText = steppedTransformer.Text;
                    Steps.WriteStep(transformer, directoryName, targetFilename, targetExtension, steppedTransformer.Current, ref lastText, newText, skipFilesWithNoChanges);
                }
            }
        }
    }
}
