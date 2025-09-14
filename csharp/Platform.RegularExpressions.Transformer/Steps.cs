using Platform.IO;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the steps.
    /// </para>
    /// </summary>
    public static class Steps
    {
        /// <summary>
        /// <para>
        /// Deletes the all steps using the specified directory name.
        /// </para>
        /// </summary>
        /// <param name="directoryName">
        /// <para>The directory name.</para>
        /// </param>
        /// <param name="targetFilename">
        /// <para>The target filename.</para>
        /// </param>
        /// <param name="targetExtension">
        /// <para>The target extension.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAllSteps(string directoryName, string targetFilename, string targetExtension)
        {
            FileHelpers.DeleteAll(directoryName, $"{targetFilename}.*.rule.txt");
            FileHelpers.DeleteAll(directoryName, $"{targetFilename}.*{targetExtension}");
        }

        /// <summary>
        /// <para>
        /// Writes the step using the specified transformer.
        /// </para>
        /// </summary>
        /// <param name="transformer">
        /// <para>The transformer.</para>
        /// </param>
        /// <param name="directoryName">
        /// <para>The directory name.</para>
        /// </param>
        /// <param name="targetFilename">
        /// <para>The target filename.</para>
        /// </param>
        /// <param name="targetExtension">
        /// <para>The target extension.</para>
        /// </param>
        /// <param name="currentStep">
        /// <para>The current step.</para>
        /// </param>
        /// <param name="lastText">
        /// <para>The last text.</para>
        /// </param>
        /// <param name="newText">
        /// <para>The new text.</para>
        /// </param>
        /// <param name="skipFilesWithNoChanges">
        /// <para>The skip files with no changes.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStep(ITransformer transformer, string directoryName, string targetFilename, string targetExtension, int currentStep, ref string lastText, string newText, bool skipFilesWithNoChanges)
        {
            if (!(skipFilesWithNoChanges && string.Equals(lastText, newText)))
            {
                lastText = newText;
                newText.WriteToFile(directoryName, $"{targetFilename}.{currentStep}{targetExtension}");
                var ruleString = transformer.Rules[currentStep].ToString();
                ruleString.WriteToFile(directoryName, $"{targetFilename}.{currentStep}.rule.txt");
            }
        }
    }
}
