#pragma once

#include "IFileTransformer.h"
#include "ITextTransformer.h"
#include <memory>
#include <string>

namespace Platform::RegularExpressions::Transformer {

/**
 * @brief File transformer implementation.
 */
class FileTransformer : public IFileTransformer {
private:
    std::shared_ptr<ITextTransformer> textTransformer_;
    std::string sourceFileExtension_;
    std::string targetFileExtension_;

public:
    /**
     * @brief Constructs a FileTransformer.
     * @param textTransformer The text transformer to use.
     * @param sourceFileExtension The source file extension to process.
     * @param targetFileExtension The target file extension to generate.
     */
    FileTransformer(std::shared_ptr<ITextTransformer> textTransformer,
                   const std::string& sourceFileExtension,
                   const std::string& targetFileExtension)
        : textTransformer_(textTransformer)
        , sourceFileExtension_(sourceFileExtension)
        , targetFileExtension_(targetFileExtension) {}

    // ITransformer interface implementation
    const std::vector<std::shared_ptr<ISubstitutionRule>>& getRules() const override {
        return textTransformer_->getRules();
    }

    // IFileTransformer interface implementation
    const std::string& getSourceFileExtension() const override {
        return sourceFileExtension_;
    }

    const std::string& getTargetFileExtension() const override {
        return targetFileExtension_;
    }

    void transform(const std::string& sourcePath, const std::string& targetPath) override;

protected:
    /**
     * @brief Transforms a single file.
     * @param sourcePath The source file path.
     * @param targetPath The target file path.
     */
    virtual void transformFile(const std::string& sourcePath, const std::string& targetPath);

    /**
     * @brief Transforms a folder recursively.
     * @param sourcePath The source folder path.
     * @param targetPath The target folder path.
     */
    virtual void transformFolder(const std::string& sourcePath, const std::string& targetPath);

private:
    /**
     * @brief Gets the target file name based on source path and target directory.
     * @param sourcePath The source file path.
     * @param targetDirectory The target directory.
     * @return The target file path.
     */
    std::string getTargetFileName(const std::string& sourcePath, const std::string& targetDirectory) const;

    /**
     * @brief Checks if a file extension matches the expected extension.
     * @param filePath The file path to check.
     * @param extension The expected extension.
     * @return true if the extension matches.
     */
    static bool fileExtensionMatches(const std::string& filePath, const std::string& extension);

    /**
     * @brief Counts files recursively with the specified extension.
     * @param path The directory path.
     * @param extension The file extension to count.
     * @return The number of files found.
     */
    static long countFilesRecursively(const std::string& path, const std::string& extension);

    /**
     * @brief Checks if a path is a directory.
     * @param path The path to check.
     * @return true if the path is a directory.
     */
    static bool isDirectory(const std::string& path);

    /**
     * @brief Checks if a path looks like a directory path (ends with separator).
     * @param path The path to check.
     * @return true if it looks like a directory path.
     */
    static bool looksLikeDirectoryPath(const std::string& path);

    /**
     * @brief Ensures that a directory exists, creating it if necessary.
     * @param path The directory path.
     */
    static void ensureDirectoryExists(const std::string& path);

    /**
     * @brief Ensures that the parent directory of a file exists.
     * @param filePath The file path.
     */
    static void ensureParentDirectoryExists(const std::string& filePath);
};

} // namespace Platform::RegularExpressions::Transformer