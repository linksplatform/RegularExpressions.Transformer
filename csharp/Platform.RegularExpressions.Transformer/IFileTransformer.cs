using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Defines the file transformer.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="ITransformer"/>
    public interface IFileTransformer : ITransformer
    {
        /// <summary>
        /// <para>
        /// Gets the source file extension value.
        /// </para>
        /// <para></para>
        /// </summary>
        string SourceFileExtension
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>
        /// Gets the target file extension value.
        /// </para>
        /// <para></para>
        /// </summary>
        string TargetFileExtension
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>
        /// Transforms the source path.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="sourcePath">
        /// <para>The source path.</para>
        /// <para></para>
        /// </param>
        /// <param name="targetPath">
        /// <para>The target path.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Transform(string sourcePath, string targetPath);
    }
}
