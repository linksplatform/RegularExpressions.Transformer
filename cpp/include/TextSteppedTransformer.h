#pragma once

#include "ITransformer.h"
#include <string>
#include <vector>
#include <memory>

namespace Platform::RegularExpressions::Transformer {

/**
 * @brief A stepped transformer that processes rules one by one.
 */
class TextSteppedTransformer : public ITransformer {
private:
    std::vector<std::shared_ptr<ISubstitutionRule>> rules_;
    std::string text_;
    int current_;

public:
    /**
     * @brief Default constructor.
     */
    TextSteppedTransformer()
        : current_(-1) {}

    /**
     * @brief Constructs with rules.
     * @param rules The substitution rules.
     */
    explicit TextSteppedTransformer(const std::vector<std::shared_ptr<ISubstitutionRule>>& rules)
        : rules_(rules), current_(-1) {}

    /**
     * @brief Constructs with rules and text.
     * @param rules The substitution rules.
     * @param text The text to transform.
     */
    TextSteppedTransformer(const std::vector<std::shared_ptr<ISubstitutionRule>>& rules, 
                          const std::string& text)
        : rules_(rules), text_(text), current_(-1) {}

    /**
     * @brief Constructs with rules, text, and current position.
     * @param rules The substitution rules.
     * @param text The text to transform.
     * @param current The current rule index.
     */
    TextSteppedTransformer(const std::vector<std::shared_ptr<ISubstitutionRule>>& rules, 
                          const std::string& text, 
                          int current)
        : rules_(rules), text_(text), current_(current) {}

    // ITransformer interface implementation
    const std::vector<std::shared_ptr<ISubstitutionRule>>& getRules() const override {
        return rules_;
    }

    /**
     * @brief Gets the current text.
     * @return The current text being transformed.
     */
    const std::string& getText() const {
        return text_;
    }

    /**
     * @brief Gets the current rule index.
     * @return The current rule index.
     */
    int getCurrent() const {
        return current_;
    }

    /**
     * @brief Sets the text.
     * @param text The text to set.
     */
    void setText(const std::string& text) {
        text_ = text;
    }

    /**
     * @brief Sets the current rule index.
     * @param current The current rule index.
     */
    void setCurrent(int current) {
        current_ = current;
    }

    /**
     * @brief Resets the transformer with new parameters.
     * @param rules The substitution rules.
     * @param text The text to transform.
     * @param current The current rule index.
     */
    void reset(const std::vector<std::shared_ptr<ISubstitutionRule>>& rules, 
              const std::string& text, 
              int current) {
        rules_ = rules;
        text_ = text;
        current_ = current;
    }

    /**
     * @brief Resets the transformer with rules and text.
     * @param rules The substitution rules.
     * @param text The text to transform.
     */
    void reset(const std::vector<std::shared_ptr<ISubstitutionRule>>& rules, 
              const std::string& text) {
        reset(rules, text, -1);
    }

    /**
     * @brief Resets with new text.
     * @param text The text to transform.
     */
    void reset(const std::string& text) {
        reset(rules_, text, -1);
    }

    /**
     * @brief Resets the transformer to initial state.
     */
    void reset() {
        reset({}, "", -1);
    }

    /**
     * @brief Processes the next rule.
     * @return true if a rule was processed, false if no more rules.
     */
    bool next();
};

} // namespace Platform::RegularExpressions::Transformer