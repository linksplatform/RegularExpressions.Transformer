#include "../include/TextSteppedTransformer.h"
#include <limits>

namespace Platform::RegularExpressions::Transformer {

bool TextSteppedTransformer::next() {
    int current = current_ + 1;
    
    if (current >= static_cast<int>(rules_.size())) {
        return false;
    }
    
    auto rule = rules_[current];
    const auto& matchPattern = rule->getMatchPattern();
    const auto& substitutionPattern = rule->getSubstitutionPattern();
    int maximumRepeatCount = rule->getMaximumRepeatCount();
    
    int replaceCount = 0;
    std::string text = text_;
    
    do {
        text = std::regex_replace(text, matchPattern, substitutionPattern);
        replaceCount++;
    } while ((maximumRepeatCount == std::numeric_limits<int>::max() || 
              replaceCount <= maximumRepeatCount) && 
             std::regex_search(text, matchPattern));
    
    text_ = text;
    current_ = current;
    return true;
}

} // namespace Platform::RegularExpressions::Transformer