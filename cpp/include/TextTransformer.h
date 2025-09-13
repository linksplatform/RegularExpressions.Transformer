#pragma once

#include "ITextTransformer.h"
#include "TextSteppedTransformer.h"

namespace Platform::RegularExpressions::Transformer {

/**
 * @brief Main text transformer implementation.
 */
class TextTransformer : public ITextTransformer {
private:
    std::vector<std::shared_ptr<ISubstitutionRule>> rules_;

public:
    /**
     * @brief Constructs with substitution rules.
     * @param substitutionRules The list of substitution rules.
     */
    explicit TextTransformer(const std::vector<std::shared_ptr<ISubstitutionRule>>& substitutionRules)
        : rules_(substitutionRules) {}

    // ITransformer interface implementation
    const std::vector<std::shared_ptr<ISubstitutionRule>>& getRules() const override {
        return rules_;
    }

    // ITextTransformer interface implementation
    std::string transform(const std::string& sourceText) override {
        TextSteppedTransformer baseTransformer(rules_);
        baseTransformer.reset(sourceText);
        
        while (baseTransformer.next()) {
            // Continue processing until all rules are applied
        }
        
        return baseTransformer.getText();
    }
};

} // namespace Platform::RegularExpressions::Transformer