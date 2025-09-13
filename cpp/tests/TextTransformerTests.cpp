#include <gtest/gtest.h>
#include "../include/TextTransformer.h"
#include "../include/SubstitutionRule.h"
#include <memory>

using namespace Platform::RegularExpressions::Transformer;

class TextTransformerTests : public ::testing::Test {
protected:
    void SetUp() override {
        // Set up any common test data here
    }
};

TEST_F(TextTransformerTests, BasicTransformation) {
    auto rule1 = std::make_shared<SubstitutionRule>("a", "b");
    auto rule2 = std::make_shared<SubstitutionRule>("b", "c");
    
    std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule1, rule2};
    TextTransformer transformer(rules);
    
    std::string sourceText = "aaaa";
    std::string result = transformer.transform(sourceText);
    
    EXPECT_EQ("cccc", result);
}

TEST_F(TextTransformerTests, NoTransformationNeeded) {
    auto rule = std::make_shared<SubstitutionRule>("x", "y");
    
    std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule};
    TextTransformer transformer(rules);
    
    std::string sourceText = "aaaa";
    std::string result = transformer.transform(sourceText);
    
    EXPECT_EQ("aaaa", result);
}

TEST_F(TextTransformerTests, PartialTransformation) {
    auto rule1 = std::make_shared<SubstitutionRule>("a", "b");
    auto rule2 = std::make_shared<SubstitutionRule>("x", "y"); // This won't match
    auto rule3 = std::make_shared<SubstitutionRule>("b", "c");
    
    std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule1, rule2, rule3};
    TextTransformer transformer(rules);
    
    std::string sourceText = "aaaa";
    std::string result = transformer.transform(sourceText);
    
    EXPECT_EQ("cccc", result);
}

TEST_F(TextTransformerTests, ComplexPattern) {
    auto rule = std::make_shared<SubstitutionRule>(R"(\d+)", "NUMBER");
    
    std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule};
    TextTransformer transformer(rules);
    
    std::string sourceText = "I have 123 apples and 456 oranges";
    std::string result = transformer.transform(sourceText);
    
    EXPECT_EQ("I have NUMBER apples and NUMBER oranges", result);
}

TEST_F(TextTransformerTests, EmptyRules) {
    std::vector<std::shared_ptr<ISubstitutionRule>> rules;
    TextTransformer transformer(rules);
    
    std::string sourceText = "test text";
    std::string result = transformer.transform(sourceText);
    
    EXPECT_EQ("test text", result);
}

TEST_F(TextTransformerTests, EmptyInput) {
    auto rule = std::make_shared<SubstitutionRule>("a", "b");
    
    std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule};
    TextTransformer transformer(rules);
    
    std::string sourceText = "";
    std::string result = transformer.transform(sourceText);
    
    EXPECT_EQ("", result);
}