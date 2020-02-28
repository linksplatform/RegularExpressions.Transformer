using System.Runtime.CompilerServices;
using Platform.Collections.Arrays;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public class TransformerCLI
    {
        private readonly IFileTransformer _transformer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TransformerCLI(IFileTransformer transformer) => _transformer = transformer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(string[] args)
        {
            var sourcePath = args.GetElementOrDefault(0);
            var targetPath = args.GetElementOrDefault(1);
            _transformer.Transform(sourcePath, targetPath);
        }
    }
}
