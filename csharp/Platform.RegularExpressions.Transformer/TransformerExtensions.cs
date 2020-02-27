using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public static class TransformerExtensions
    {
        public static List<string> GetSteps(this Transformer transformer, string source) => transformer.GenerateTransformersForEachRulesStep().TransformWithAll(source);

        public static void WriteStepsToFiles(this Transformer transformer, string sourcePath, string targetFilename, string targetExtension, bool skipFilesWithNoChanges) => transformer.GenerateTransformersForEachRulesStep().TransformWithAllToFiles(sourcePath, targetFilename, targetExtension, skipFilesWithNoChanges);
    }
}
