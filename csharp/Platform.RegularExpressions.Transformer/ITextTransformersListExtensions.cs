using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform.Collections;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the text transformers list extensions.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class ITextTransformersListExtensions
    {
        /// <summary>
        /// <para>
        /// Transforms the with all using the specified transformers.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="transformers">
        /// <para>The transformers.</para>
        /// <para></para>
        /// </param>
        /// <param name="source">
        /// <para>The source.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>A list of string</para>
        /// <para></para>
        /// </returns>
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

        /// <summary>
        /// <para>
        /// Transforms the with all to files using the specified transformers.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="transformers">
        /// <para>The transformers.</para>
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
        public static void TransformWithAllToFiles(this IList<ITextTransformer> transformers, string sourceText, string targetPath, bool skipFilesWithNoChanges)
        {
            if (!transformers.IsNullOrEmpty())
            {
                targetPath.GetPathParts(out var directoryName, out var targetFilename, out var targetExtension);
                Steps.DeleteAllSteps(directoryName, targetFilename, targetExtension);
                var lastText = "";
                for (int i = 0; i < transformers.Count; i++)
                {
                    var transformer = transformers[i];
                    var newText = transformer.Transform(sourceText);
                    Steps.WriteStep(transformer, directoryName, targetFilename, targetExtension, i, ref lastText, newText, skipFilesWithNoChanges);
                }
            }
        }
    }
}
