#pragma once

#include "ITransformer.h"
#include <string>

namespace Platform::RegularExpressions::Transformer {

/**
 * @brief Defines the file transformer interface.
 */
class IFileTransformer : public ITransformer {
public:
    virtual ~IFileTransformer() = default;

    /**
     * @brief Gets the source file extension.
     * @return The source file extension.
     */
    virtual const std::string& getSourceFileExtension() const = 0;

    /**
     * @brief Gets the target file extension.
     * @return The target file extension.
     */
    virtual const std::string& getTargetFileExtension() const = 0;

    /**
     * @brief Transforms files from source path to target path.
     * @param sourcePath The source file or directory path.
     * @param targetPath The target file or directory path.
     */
    virtual void transform(const std::string& sourcePath, const std::string& targetPath) = 0;
};

} // namespace Platform::RegularExpressions::Transformer