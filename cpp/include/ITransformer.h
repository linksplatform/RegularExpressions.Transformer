#pragma once

#include "ISubstitutionRule.h"
#include <vector>
#include <memory>

namespace Platform::RegularExpressions::Transformer {

/**
 * @brief Defines the base transformer interface.
 */
class ITransformer {
public:
    virtual ~ITransformer() = default;

    /**
     * @brief Gets the list of substitution rules.
     * @return Reference to the rules vector.
     */
    virtual const std::vector<std::shared_ptr<ISubstitutionRule>>& getRules() const = 0;
};

} // namespace Platform::RegularExpressions::Transformer