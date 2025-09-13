#pragma once

#include "ITransformer.h"
#include <string>

namespace Platform::RegularExpressions::Transformer {

/**
 * @brief Defines the text transformer interface.
 */
class ITextTransformer : public ITransformer {
public:
    virtual ~ITextTransformer() = default;

    /**
     * @brief Transforms the source text using the configured rules.
     * @param sourceText The text to transform.
     * @return The transformed text.
     */
    virtual std::string transform(const std::string& sourceText) = 0;
};

} // namespace Platform::RegularExpressions::Transformer