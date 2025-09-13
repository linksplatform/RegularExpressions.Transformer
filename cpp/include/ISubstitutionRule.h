#pragma once

#include <regex>
#include <string>

namespace Platform::RegularExpressions::Transformer {

/**
 * @brief Defines the substitution rule interface.
 */
class ISubstitutionRule {
public:
    virtual ~ISubstitutionRule() = default;

    /**
     * @brief Gets the match pattern.
     * @return The regex match pattern.
     */
    virtual const std::regex& getMatchPattern() const = 0;

    /**
     * @brief Gets the substitution pattern.
     * @return The substitution pattern string.
     */
    virtual const std::string& getSubstitutionPattern() const = 0;

    /**
     * @brief Gets the maximum repeat count.
     * @return The maximum repeat count.
     */
    virtual int getMaximumRepeatCount() const = 0;

    /**
     * @brief Gets the path pattern.
     * @return The regex path pattern (optional).
     */
    virtual const std::regex* getPathPattern() const = 0;
};

} // namespace Platform::RegularExpressions::Transformer