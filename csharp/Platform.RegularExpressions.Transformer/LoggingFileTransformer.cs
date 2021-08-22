using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the logging file transformer.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="FileTransformer"/>
    public class LoggingFileTransformer : FileTransformer
    {
        /// <summary>
        /// <para>
        /// Initializes a new <see cref="LoggingFileTransformer"/> instance.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="textTransformer">
        /// <para>A text transformer.</para>
        /// <para></para>
        /// </param>
        /// <param name="sourceFileExtension">
        /// <para>A source file extension.</para>
        /// <para></para>
        /// </param>
        /// <param name="targetFileExtension">
        /// <para>A target file extension.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LoggingFileTransformer(ITextTransformer textTransformer, string sourceFileExtension, string targetFileExtension) : base(textTransformer, sourceFileExtension, targetFileExtension) { }

        /// <summary>
        /// <para>
        /// Transforms the file using the specified source path.
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
        protected override void TransformFile(string sourcePath, string targetPath)
        {
            base.TransformFile(sourcePath, targetPath);
            // Logging
            var sourceText = File.ReadAllText(sourcePath, Encoding.UTF8);
            _textTransformer.WriteStepsToFiles(sourceText, targetPath, skipFilesWithNoChanges: true);
        }
    }
}
