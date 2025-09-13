#include "../include/TextTransformer.h"
#include "../include/SubstitutionRule.h"
#include <iostream>
#include <memory>

using namespace Platform::RegularExpressions::Transformer;

int main() {
    try {
        // Create substitution rules
        auto rule1 = std::make_shared<SubstitutionRule>("hello", "hi");
        auto rule2 = std::make_shared<SubstitutionRule>("world", "universe");
        auto rule3 = std::make_shared<SubstitutionRule>(R"(\d+)", "NUMBER");
        
        // Create a vector of rules
        std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule1, rule2, rule3};
        
        // Create the transformer
        TextTransformer transformer(rules);
        
        // Test text
        std::string sourceText = "hello world! I have 123 apples and 456 oranges.";
        
        std::cout << "Source text: " << sourceText << std::endl;
        
        // Transform the text
        std::string result = transformer.transform(sourceText);
        
        std::cout << "Transformed text: " << result << std::endl;
        
        // Expected output: "hi universe! I have NUMBER apples and NUMBER oranges."
        
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << std::endl;
        return 1;
    }
    
    return 0;
}