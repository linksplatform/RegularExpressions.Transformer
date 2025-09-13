#pragma once

#include "ISubstitutionRule.h"
#include <regex>
#include <string>
#include <memory>
#include <chrono>
#include <sstream>
#include <limits>

namespace Platform::RegularExpressions::Transformer {

/**
 * @brief Represents a substitution rule implementation.
 */
class SubstitutionRule : public ISubstitutionRule {
public:
    static constexpr std::regex_constants::syntax_option_type DEFAULT_REGEX_OPTIONS = 
        std::regex_constants::ECMAScript | std::regex_constants::multiline;

private:
    std::regex matchPattern_;
    std::string substitutionPattern_;
    std::unique_ptr<std::regex> pathPattern_;
    int maximumRepeatCount_;

public:
    /**
     * @brief Constructs a SubstitutionRule with regex objects.
     * @param matchPattern The regex pattern to match.
     * @param substitutionPattern The substitution string.
     * @param maximumRepeatCount Maximum number of repetitions (0 = no limit).
     */
    SubstitutionRule(const std::regex& matchPattern, 
                     const std::string& substitutionPattern, 
                     int maximumRepeatCount = 0)
        : matchPattern_(matchPattern)
        , substitutionPattern_(substitutionPattern)
        , pathPattern_(nullptr)
        , maximumRepeatCount_(maximumRepeatCount) {}

    /**
     * @brief Constructs a SubstitutionRule with string patterns.
     * @param matchPatternStr The regex pattern string to match.
     * @param substitutionPattern The substitution string.
     * @param maximumRepeatCount Maximum number of repetitions (0 = no limit).
     * @param regexOptions Regex compilation options.
     */
    SubstitutionRule(const std::string& matchPatternStr,
                     const std::string& substitutionPattern,
                     int maximumRepeatCount = 0,
                     std::regex_constants::syntax_option_type regexOptions = DEFAULT_REGEX_OPTIONS)
        : matchPattern_(matchPatternStr, regexOptions)
        , substitutionPattern_(substitutionPattern)
        , pathPattern_(nullptr)
        , maximumRepeatCount_(maximumRepeatCount) {}

    /**
     * @brief Copy constructor.
     */
    SubstitutionRule(const SubstitutionRule& other)
        : matchPattern_(other.matchPattern_)
        , substitutionPattern_(other.substitutionPattern_)
        , pathPattern_(other.pathPattern_ ? std::make_unique<std::regex>(*other.pathPattern_) : nullptr)
        , maximumRepeatCount_(other.maximumRepeatCount_) {}

    /**
     * @brief Copy assignment operator.
     */
    SubstitutionRule& operator=(const SubstitutionRule& other) {
        if (this != &other) {
            matchPattern_ = other.matchPattern_;
            substitutionPattern_ = other.substitutionPattern_;
            pathPattern_ = other.pathPattern_ ? std::make_unique<std::regex>(*other.pathPattern_) : nullptr;
            maximumRepeatCount_ = other.maximumRepeatCount_;
        }
        return *this;
    }

    /**
     * @brief Move constructor.
     */
    SubstitutionRule(SubstitutionRule&& other) noexcept
        : matchPattern_(std::move(other.matchPattern_))
        , substitutionPattern_(std::move(other.substitutionPattern_))
        , pathPattern_(std::move(other.pathPattern_))
        , maximumRepeatCount_(other.maximumRepeatCount_) {}

    /**
     * @brief Move assignment operator.
     */
    SubstitutionRule& operator=(SubstitutionRule&& other) noexcept {
        if (this != &other) {
            matchPattern_ = std::move(other.matchPattern_);
            substitutionPattern_ = std::move(other.substitutionPattern_);
            pathPattern_ = std::move(other.pathPattern_);
            maximumRepeatCount_ = other.maximumRepeatCount_;
        }
        return *this;
    }

    // ISubstitutionRule interface implementation
    const std::regex& getMatchPattern() const override {
        return matchPattern_;
    }

    const std::string& getSubstitutionPattern() const override {
        return substitutionPattern_;
    }

    int getMaximumRepeatCount() const override {
        return maximumRepeatCount_;
    }

    const std::regex* getPathPattern() const override {
        return pathPattern_.get();
    }

    /**
     * @brief Sets the path pattern.
     * @param pathPattern The path pattern regex.
     */
    void setPathPattern(const std::regex& pathPattern) {
        pathPattern_ = std::make_unique<std::regex>(pathPattern);
    }

    /**
     * @brief Sets the path pattern from string.
     * @param pathPatternStr The path pattern string.
     * @param regexOptions Regex compilation options.
     */
    void setPathPattern(const std::string& pathPatternStr,
                       std::regex_constants::syntax_option_type regexOptions = DEFAULT_REGEX_OPTIONS) {
        pathPattern_ = std::make_unique<std::regex>(pathPatternStr, regexOptions);
    }

    /**
     * @brief Sets the maximum repeat count.
     * @param count The maximum repeat count.
     */
    void setMaximumRepeatCount(int count) {
        maximumRepeatCount_ = count;
    }

    /**
     * @brief Returns string representation of the rule.
     * @return String representation.
     */
    std::string toString() const {
        std::ostringstream ss;
        ss << "\"" << "pattern" << "\" -> \"" << substitutionPattern_ << "\"";
        
        if (pathPattern_) {
            ss << " on files matching path pattern";
        }
        
        if (maximumRepeatCount_ > 0) {
            if (maximumRepeatCount_ == std::numeric_limits<int>::max()) {
                ss << " repeated forever";
            } else {
                ss << " repeated up to " << maximumRepeatCount_ << " times";
            }
        }
        
        return ss.str();
    }
};

} // namespace Platform::RegularExpressions::Transformer