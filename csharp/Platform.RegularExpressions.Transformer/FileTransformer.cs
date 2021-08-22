using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.RegularExpressions.Transformer
{
    /// <summary>
    /// <para>
    /// Represents the file transformer.
    /// </para>
    /// <para></para>
    /// </summary>
    /// <seealso cref="IFileTransformer"/>
    public class FileTransformer : IFileTransformer
    {
        /// <summary>
        /// <para>
        /// The text transformer.
        /// </para>
        /// <para></para>
        /// </summary>
        protected readonly ITextTransformer _textTransformer;

        /// <summary>
        /// <para>
        /// Gets or sets the source file extension value.
        /// </para>
        /// <para></para>
        /// </summary>
        public string SourceFileExtension
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        /// <summary>
        /// <para>
        /// Gets or sets the target file extension value.
        /// </para>
        /// <para></para>
        /// </summary>
        public string TargetFileExtension
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        /// <summary>
        /// <para>
        /// Gets the rules value.
        /// </para>
        /// <para></para>
        /// </summary>
        public IList<ISubstitutionRule> Rules
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _textTransformer.Rules;
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="FileTransformer"/> instance.
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
        public FileTransformer(ITextTransformer textTransformer, string sourceFileExtension, string targetFileExtension)
        {
            _textTransformer = textTransformer;
            SourceFileExtension = sourceFileExtension;
            TargetFileExtension = targetFileExtension;
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
        /// <exception cref="NotSupportedException">
        /// <para></para>
        /// <para></para>
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Transform(string sourcePath, string targetPath)
        {
            var defaultPath = Path.GetFullPath(".");
            if (string.IsNullOrEmpty(sourcePath))
            {
                sourcePath = defaultPath;
            }
            if (string.IsNullOrEmpty(targetPath))
            {
                targetPath = defaultPath;
            }
            var sourceDirectoryExists = DirectoryExists(sourcePath);
            var sourceDirectoryPath = LooksLikeDirectoryPath(sourcePath);
            var sourceIsDirectory = sourceDirectoryExists || sourceDirectoryPath;
            var targetDirectoryExists = DirectoryExists(targetPath);
            var targetDirectoryPath = LooksLikeDirectoryPath(targetPath);
            var targetIsDirectory = targetDirectoryExists || targetDirectoryPath;
            if (sourceIsDirectory && targetIsDirectory)
            {
                // Folder -> Folder
                if (!sourceDirectoryExists)
                {
                    return;
                }
                TransformFolder(sourcePath, targetPath);
            }
            else if (!(sourceIsDirectory || targetIsDirectory))
            {
                // File -> File
                EnsureSourceFileExists(sourcePath);
                EnsureTargetFileDirectoryExists(targetPath);
                TransformFile(sourcePath, targetPath);
            }
            else if (targetIsDirectory)
            {
                // File -> Folder
                EnsureSourceFileExists(sourcePath);
                EnsureTargetDirectoryExists(targetPath, targetDirectoryExists);
                TransformFile(sourcePath, GetTargetFileName(sourcePath, targetPath));
            }
            else
            {
                // Folder -> File
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// <para>
        /// Transforms the folder using the specified source path.
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
        protected virtual void TransformFolder(string sourcePath, string targetPath)
        {
            if (CountFilesRecursively(sourcePath, SourceFileExtension) == 0)
            {
                return;
            }
            EnsureTargetDirectoryExists(targetPath);
            var directories = Directory.GetDirectories(sourcePath);
            for (var i = 0; i < directories.Length; i++)
            {
#if NETSTANDARD2_1
                var relativePath = Path.GetRelativePath(sourcePath, directories[i]);
#else
                var relativePath = directories[i].Replace(sourcePath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, "");
#endif
                var newTargetPath = Path.Combine(targetPath, relativePath);
                TransformFolder(directories[i], newTargetPath);
            }
            var files = Directory.GetFiles(sourcePath);
            Parallel.For(0, files.Length, i =>
            {
                var file = files[i];
                if (FileExtensionMatches(file, SourceFileExtension))
                {
                    TransformFile(file, GetTargetFileName(file, targetPath));
                }
            });
        }

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
        protected virtual void TransformFile(string sourcePath, string targetPath)
        {
            if (File.Exists(targetPath))
            {
                var applicationPath = Process.GetCurrentProcess().MainModule.FileName;
                var targetFileLastUpdateDateTime = new FileInfo(targetPath).LastWriteTimeUtc;
                if (new FileInfo(sourcePath).LastWriteTimeUtc < targetFileLastUpdateDateTime && new FileInfo(applicationPath).LastWriteTimeUtc < targetFileLastUpdateDateTime)
                {
                    return;
                }
            }
            var sourceText = File.ReadAllText(sourcePath, Encoding.UTF8);
            var targetText = _textTransformer.Transform(sourceText);
            File.WriteAllText(targetPath, targetText, Encoding.UTF8);
        }

        /// <summary>
        /// <para>
        /// Gets the target file name using the specified source path.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="sourcePath">
        /// <para>The source path.</para>
        /// <para></para>
        /// </param>
        /// <param name="targetDirectory">
        /// <para>The target directory.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The string</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string GetTargetFileName(string sourcePath, string targetDirectory) => Path.ChangeExtension(Path.Combine(targetDirectory, Path.GetFileName(sourcePath)), TargetFileExtension);

        /// <summary>
        /// <para>
        /// Counts the files recursively using the specified path.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="path">
        /// <para>The path.</para>
        /// <para></para>
        /// </param>
        /// <param name="extension">
        /// <para>The extension.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The result.</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long CountFilesRecursively(string path, string extension)
        {
            var files = Directory.GetFiles(path);
            var directories = Directory.GetDirectories(path);
            var result = 0L;
            for (var i = 0; i < directories.Length; i++)
            {
                result += CountFilesRecursively(directories[i], extension);
            }
            for (var i = 0; i < files.Length; i++)
            {
                if (FileExtensionMatches(files[i], extension))
                {
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// <para>
        /// Determines whether file extension matches.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="file">
        /// <para>The file.</para>
        /// <para></para>
        /// </param>
        /// <param name="extension">
        /// <para>The extension.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool FileExtensionMatches(string file, string extension) => file.EndsWith(extension, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// <para>
        /// Ensures the target file directory exists using the specified target path.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="targetPath">
        /// <para>The target path.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureTargetFileDirectoryExists(string targetPath)
        {
            if (!File.Exists(targetPath))
            {
                EnsureDirectoryIsCreated(targetPath);
            }
        }

        /// <summary>
        /// <para>
        /// Ensures the target directory exists using the specified target path.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="targetPath">
        /// <para>The target path.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureTargetDirectoryExists(string targetPath) => EnsureTargetDirectoryExists(targetPath, DirectoryExists(targetPath));

        /// <summary>
        /// <para>
        /// Ensures the target directory exists using the specified target path.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="targetPath">
        /// <para>The target path.</para>
        /// <para></para>
        /// </param>
        /// <param name="targetDirectoryExists">
        /// <para>The target directory exists.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureTargetDirectoryExists(string targetPath, bool targetDirectoryExists)
        {
            if (!targetDirectoryExists)
            {
                Directory.CreateDirectory(targetPath);
            }
        }

        /// <summary>
        /// <para>
        /// Ensures the source file exists using the specified source path.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="sourcePath">
        /// <para>The source path.</para>
        /// <para></para>
        /// </param>
        /// <exception cref="FileNotFoundException">
        /// <para>Source file does not exists. </para>
        /// <para></para>
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureSourceFileExists(string sourcePath)
        {
            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException("Source file does not exists.", sourcePath);
            }
        }

        /// <summary>
        /// <para>
        /// Ensures the directory is created using the specified target path.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="targetPath">
        /// <para>The target path.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureDirectoryIsCreated(string targetPath) => Directory.CreateDirectory(Path.GetDirectoryName(targetPath));

        /// <summary>
        /// <para>
        /// Determines whether directory exists.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="path">
        /// <para>The path.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool DirectoryExists(string path) => Directory.Exists(path) && File.GetAttributes(path).HasFlag(FileAttributes.Directory);

        /// <summary>
        /// <para>
        /// Determines whether looks like directory path.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="path">
        /// <para>The path.</para>
        /// <para></para>
        /// </param>
        /// <returns>
        /// <para>The bool</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool LooksLikeDirectoryPath(string path) => path.EndsWith(Path.DirectorySeparatorChar.ToString()) || path.EndsWith(Path.AltDirectorySeparatorChar.ToString());
    }
}
