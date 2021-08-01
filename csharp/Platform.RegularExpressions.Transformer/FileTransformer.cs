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
    public class FileTransformer : IFileTransformer
    {
        protected readonly ITextTransformer _textTransformer;

        public string SourceFileExtension
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        public string TargetFileExtension
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        public IList<ISubstitutionRule> Rules
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _textTransformer.Rules;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FileTransformer(ITextTransformer textTransformer, string sourceFileExtension, string targetFileExtension)
        {
            _textTransformer = textTransformer;
            SourceFileExtension = sourceFileExtension;
            TargetFileExtension = targetFileExtension;
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string GetTargetFileName(string sourcePath, string targetDirectory) => Path.ChangeExtension(Path.Combine(targetDirectory, Path.GetFileName(sourcePath)), TargetFileExtension);

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool FileExtensionMatches(string file, string extension) => file.EndsWith(extension, StringComparison.OrdinalIgnoreCase);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureTargetFileDirectoryExists(string targetPath)
        {
            if (!File.Exists(targetPath))
            {
                EnsureDirectoryIsCreated(targetPath);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureTargetDirectoryExists(string targetPath) => EnsureTargetDirectoryExists(targetPath, DirectoryExists(targetPath));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureTargetDirectoryExists(string targetPath, bool targetDirectoryExists)
        {
            if (!targetDirectoryExists)
            {
                Directory.CreateDirectory(targetPath);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureSourceFileExists(string sourcePath)
        {
            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException("Source file does not exists.", sourcePath);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void EnsureDirectoryIsCreated(string targetPath) => Directory.CreateDirectory(Path.GetDirectoryName(targetPath));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool DirectoryExists(string path) => Directory.Exists(path) && File.GetAttributes(path).HasFlag(FileAttributes.Directory);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool LooksLikeDirectoryPath(string path) => path.EndsWith(Path.DirectorySeparatorChar.ToString()) || path.EndsWith(Path.AltDirectorySeparatorChar.ToString());
    }
}
