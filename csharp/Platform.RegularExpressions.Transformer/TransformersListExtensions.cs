﻿using System.IO;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public static class TransformersListExtensions
    {
        public static List<string> TransformWithAll(this IList<ITransformer> transformers, string source)
        {
            var strings = new List<string>();
            if (transformers.Count > 0)
            {
                for (int i = 0; i < transformers.Count; i++)
                {
                    strings.Add(transformers[i].Transform(source, null));
                }
            }
            return strings;
        }

        public static void TransformWithAllToFiles(this IList<ITransformer> transformers, string sourcePath, string targetFilename, string targetExtension)
        {
            if (transformers.Count > 0)
            {
                var sourceText = File.ReadAllText(sourcePath, Encoding.UTF8);
                var transformerContext = new Context(sourcePath);
                for (int i = 0; i < transformers.Count; i++)
                {
                    var transformationOutput = transformers[i].Transform(sourceText, transformerContext);
                    File.WriteAllText($"{targetFilename}.{i}{targetExtension}", transformationOutput, Encoding.UTF8);
                }
            }
        }
    }
}
