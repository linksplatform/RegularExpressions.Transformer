#include "../include/FileTransformer.h"
#include "../include/TextTransformer.h"
#include "../include/SubstitutionRule.h"
#include <iostream>
#include <fstream>
#include <memory>
#include <filesystem>

using namespace Platform::RegularExpressions::Transformer;

int main() {
    try {
        // Create a temporary directory for testing
        std::string tempDir = "temp_example";
        std::filesystem::create_directories(tempDir);
        
        // Create a source file
        std::string sourceFile = tempDir + "/source.txt";
        std::ofstream file(sourceFile);
        file << "Hello World!\n";
        file << "This is a test file with some numbers: 123, 456, 789.\n";
        file << "We will transform this content.\n";
        file.close();
        
        // Create substitution rules
        auto rule1 = std::make_shared<SubstitutionRule>("Hello", "Hi");
        auto rule2 = std::make_shared<SubstitutionRule>("World", "Universe");
        auto rule3 = std::make_shared<SubstitutionRule>(R"(\b\d+\b)", "[NUMBER]");
        
        std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule1, rule2, rule3};
        
        // Create transformers
        auto textTransformer = std::make_shared<TextTransformer>(rules);
        FileTransformer fileTransformer(textTransformer, ".txt", ".transformed");
        
        // Transform the file
        std::string targetFile = tempDir + "/source.transformed";
        fileTransformer.transform(sourceFile, targetFile);
        
        // Read and display the results
        std::cout << "Original file content:" << std::endl;
        std::ifstream original(sourceFile);
        std::string line;
        while (std::getline(original, line)) {
            std::cout << "  " << line << std::endl;
        }
        original.close();
        
        std::cout << std::endl << "Transformed file content:" << std::endl;
        std::ifstream transformed(targetFile);
        while (std::getline(transformed, line)) {
            std::cout << "  " << line << std::endl;
        }
        transformed.close();
        
        // Clean up
        std::filesystem::remove_all(tempDir);
        
        std::cout << std::endl << "Example completed successfully!" << std::endl;
        
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << std::endl;
        return 1;
    }
    
    return 0;
}