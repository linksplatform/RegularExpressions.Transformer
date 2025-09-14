using System.Runtime.CompilerServices;
using Platform.Collections.Arrays;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the transformer cli.
    /// </para>
    /// </summary>
    public class TransformerCLI
    {
        /// <summary>
        /// <para>
        /// The transformer.
        /// </para>
        /// </summary>
        private readonly IFileTransformer _transformer;

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="TransformerCLI"/> instance.
        /// </para>
        /// </summary>
        /// <param name="transformer">
        /// <para>A transformer.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TransformerCLI(IFileTransformer transformer) => _transformer = transformer;

        /// <summary>
        /// <para>
        /// Runs the args.
        /// </para>
        /// </summary>
        /// <param name="args">
        /// <para>The args.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(string[] args)
        {
            var sourcePath = args.GetElementOrDefault(0);
            var targetPath = args.GetElementOrDefault(1);
            _transformer.Transform(sourcePath, targetPath);
        }
    }
}
