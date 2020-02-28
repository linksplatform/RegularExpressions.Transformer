using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    public class LoggingFileTransformer : FileTransformer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LoggingFileTransformer(ITextTransformer textTransformer, string sourceFileExtension, string targetFileExtension) : base(textTransformer, sourceFileExtension, targetFileExtension) { }

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
