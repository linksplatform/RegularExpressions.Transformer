#include "../include/FileTransformer.h"
#include <filesystem>
#include <fstream>
#include <sstream>
#include <stdexcept>
#include <algorithm>
#include <thread>
#include <future>
#include <vector>

namespace Platform::RegularExpressions::Transformer {

void FileTransformer::transform(const std::string& sourcePath, const std::string& targetPath) {
    std::filesystem::path defaultPath = std::filesystem::current_path();
    
    std::string actualSourcePath = sourcePath.empty() ? defaultPath.string() : sourcePath;
    std::string actualTargetPath = targetPath.empty() ? defaultPath.string() : targetPath;
    
    bool sourceIsDirectory = isDirectory(actualSourcePath) || looksLikeDirectoryPath(actualSourcePath);
    bool targetIsDirectory = isDirectory(actualTargetPath) || looksLikeDirectoryPath(actualTargetPath);
    
    if (sourceIsDirectory && targetIsDirectory) {
        // Folder -> Folder
        if (!std::filesystem::exists(actualSourcePath)) {
            return;
        }
        transformFolder(actualSourcePath, actualTargetPath);
    } else if (!(sourceIsDirectory || targetIsDirectory)) {
        // File -> File
        if (!std::filesystem::exists(actualSourcePath)) {
            throw std::runtime_error("Source file does not exist: " + actualSourcePath);
        }
        ensureParentDirectoryExists(actualTargetPath);
        transformFile(actualSourcePath, actualTargetPath);
    } else if (targetIsDirectory) {
        // File -> Folder
        if (!std::filesystem::exists(actualSourcePath)) {
            throw std::runtime_error("Source file does not exist: " + actualSourcePath);
        }
        ensureDirectoryExists(actualTargetPath);
        transformFile(actualSourcePath, getTargetFileName(actualSourcePath, actualTargetPath));
    } else {
        // Folder -> File
        throw std::runtime_error("Cannot transform folder to file");
    }
}

void FileTransformer::transformFile(const std::string& sourcePath, const std::string& targetPath) {
    // Check if target file is newer than source and application
    if (std::filesystem::exists(targetPath)) {
        auto targetTime = std::filesystem::last_write_time(targetPath);
        auto sourceTime = std::filesystem::last_write_time(sourcePath);
        
        if (sourceTime < targetTime) {
            return; // Skip if target is newer
        }
    }
    
    // Read source file
    std::ifstream sourceFile(sourcePath);
    if (!sourceFile) {
        throw std::runtime_error("Cannot open source file: " + sourcePath);
    }
    
    std::stringstream buffer;
    buffer << sourceFile.rdbuf();
    std::string sourceText = buffer.str();
    sourceFile.close();
    
    // Transform text
    std::string targetText = textTransformer_->transform(sourceText);
    
    // Write target file
    std::ofstream targetFile(targetPath);
    if (!targetFile) {
        throw std::runtime_error("Cannot create target file: " + targetPath);
    }
    
    targetFile << targetText;
    targetFile.close();
}

void FileTransformer::transformFolder(const std::string& sourcePath, const std::string& targetPath) {
    if (countFilesRecursively(sourcePath, sourceFileExtension_) == 0) {
        return;
    }
    
    ensureDirectoryExists(targetPath);
    
    // Process subdirectories
    for (const auto& entry : std::filesystem::directory_iterator(sourcePath)) {
        if (entry.is_directory()) {
            std::filesystem::path relativePath = std::filesystem::relative(entry.path(), sourcePath);
            std::filesystem::path newTargetPath = std::filesystem::path(targetPath) / relativePath;
            transformFolder(entry.path().string(), newTargetPath.string());
        }
    }
    
    // Process files in parallel
    std::vector<std::future<void>> futures;
    
    for (const auto& entry : std::filesystem::directory_iterator(sourcePath)) {
        if (entry.is_regular_file()) {
            std::string filePath = entry.path().string();
            if (fileExtensionMatches(filePath, sourceFileExtension_)) {
                futures.push_back(std::async(std::launch::async, [this, filePath, targetPath]() {
                    transformFile(filePath, getTargetFileName(filePath, targetPath));
                }));
            }
        }
    }
    
    // Wait for all tasks to complete
    for (auto& future : futures) {
        future.wait();
    }
}

std::string FileTransformer::getTargetFileName(const std::string& sourcePath, const std::string& targetDirectory) const {
    std::filesystem::path source(sourcePath);
    std::filesystem::path target(targetDirectory);
    
    std::filesystem::path filename = source.filename();
    filename.replace_extension(targetFileExtension_);
    
    return (target / filename).string();
}

bool FileTransformer::fileExtensionMatches(const std::string& filePath, const std::string& extension) {
    std::filesystem::path path(filePath);
    std::string fileExt = path.extension().string();
    
    // Case-insensitive comparison
    std::string lowerFileExt = fileExt;
    std::string lowerExpectedExt = extension;
    
    std::transform(lowerFileExt.begin(), lowerFileExt.end(), lowerFileExt.begin(), ::tolower);
    std::transform(lowerExpectedExt.begin(), lowerExpectedExt.end(), lowerExpectedExt.begin(), ::tolower);
    
    return lowerFileExt == lowerExpectedExt;
}

long FileTransformer::countFilesRecursively(const std::string& path, const std::string& extension) {
    long count = 0;
    
    try {
        for (const auto& entry : std::filesystem::recursive_directory_iterator(path)) {
            if (entry.is_regular_file() && fileExtensionMatches(entry.path().string(), extension)) {
                count++;
            }
        }
    } catch (const std::filesystem::filesystem_error&) {
        // Directory might not exist or be accessible
        return 0;
    }
    
    return count;
}

bool FileTransformer::isDirectory(const std::string& path) {
    return std::filesystem::exists(path) && std::filesystem::is_directory(path);
}

bool FileTransformer::looksLikeDirectoryPath(const std::string& path) {
    return path.back() == '/' || path.back() == '\\';
}

void FileTransformer::ensureDirectoryExists(const std::string& path) {
    if (!std::filesystem::exists(path)) {
        std::filesystem::create_directories(path);
    }
}

void FileTransformer::ensureParentDirectoryExists(const std::string& filePath) {
    std::filesystem::path path(filePath);
    std::filesystem::path parentPath = path.parent_path();
    
    if (!parentPath.empty() && !std::filesystem::exists(parentPath)) {
        std::filesystem::create_directories(parentPath);
    }
}

} // namespace Platform::RegularExpressions::Transformer