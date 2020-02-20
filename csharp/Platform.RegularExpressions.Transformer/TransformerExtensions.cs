#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public static class TransformerExtensions
    {
        public static void DebugOutput(this Transformer transformer, string sourcePath, string targetFilename, string targetExtension) => transformer.GenerateTransformersForEachRulesStep().TransformAllToFiles(sourcePath, targetFilename, targetExtension);
    }
}
