using Platform.IO;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public static class Steps
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAllSteps(string directoryName, string targetFilename, string targetExtension)
        {
            FileHelpers.DeleteAll(directoryName, $"{targetFilename}.*.rule.txt");
            FileHelpers.DeleteAll(directoryName, $"{targetFilename}.*{targetExtension}");
        }

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
