using System.Runtime.CompilerServices;
using Platform.Collections.Arrays;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the transformer cli.
    /// </para>
    /// <para></para>
    /// </summary>
    public class TransformerCLI
    {
        private readonly IFileTransformer _transformer;

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="TransformerCLI"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="transformer">
        /// <para>A transformer.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TransformerCLI(IFileTransformer transformer) => _transformer = transformer;

        /// <summary>
        /// <para>
        /// Runs the args.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="args">
        /// <para>The args.</para>
        /// <para></para>
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
