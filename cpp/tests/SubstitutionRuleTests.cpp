#include <gtest/gtest.h>
#include "../include/SubstitutionRule.h"

using namespace Platform::RegularExpressions::Transformer;

class SubstitutionRuleTests : public ::testing::Test {
protected:
    void SetUp() override {
        // Set up any common test data here
    }
};

TEST_F(SubstitutionRuleTests, ConstructorWithStringPattern) {
    std::string pattern = R"(^\s*?\#pragma[\sa-zA-Z0-9\/]+$)";
    std::string substitution = "";
    
    SubstitutionRule rule(pattern, substitution);
    
    EXPECT_EQ(substitution, rule.getSubstitutionPattern());
    EXPECT_EQ(0, rule.getMaximumRepeatCount());
    EXPECT_EQ(nullptr, rule.getPathPattern());
}

TEST_F(SubstitutionRuleTests, ConstructorWithRegexPattern) {
    std::regex pattern(R"(\d+)");
    std::string substitution = "number";
    int maxRepeat = 5;
    
    SubstitutionRule rule(pattern, substitution, maxRepeat);
    
    EXPECT_EQ(substitution, rule.getSubstitutionPattern());
    EXPECT_EQ(maxRepeat, rule.getMaximumRepeatCount());
    EXPECT_EQ(nullptr, rule.getPathPattern());
}

TEST_F(SubstitutionRuleTests, CopyConstructor) {
    std::string pattern = R"([a-z]+)";
    std::string substitution = "word";
    int maxRepeat = 3;
    
    SubstitutionRule original(pattern, substitution, maxRepeat);
    original.setPathPattern(R"(.*\.txt$)");
    
    SubstitutionRule copy(original);
    
    EXPECT_EQ(original.getSubstitutionPattern(), copy.getSubstitutionPattern());
    EXPECT_EQ(original.getMaximumRepeatCount(), copy.getMaximumRepeatCount());
    EXPECT_NE(nullptr, copy.getPathPattern());
}

TEST_F(SubstitutionRuleTests, MoveConstructor) {
    std::string pattern = R"([A-Z]+)";
    std::string substitution = "WORD";
    
    SubstitutionRule original(pattern, substitution);
    SubstitutionRule moved(std::move(original));
    
    EXPECT_EQ(substitution, moved.getSubstitutionPattern());
    EXPECT_EQ(0, moved.getMaximumRepeatCount());
}

TEST_F(SubstitutionRuleTests, SetPathPattern) {
    SubstitutionRule rule("test", "replacement");
    std::string pathPattern = R"(.*\.cpp$)";
    
    rule.setPathPattern(pathPattern);
    
    EXPECT_NE(nullptr, rule.getPathPattern());
}

TEST_F(SubstitutionRuleTests, SetMaximumRepeatCount) {
    SubstitutionRule rule("test", "replacement");
    int newCount = 10;
    
    rule.setMaximumRepeatCount(newCount);
    
    EXPECT_EQ(newCount, rule.getMaximumRepeatCount());
}

TEST_F(SubstitutionRuleTests, ToStringBasic) {
    SubstitutionRule rule("test", "replacement");
    
    std::string result = rule.toString();
    
    EXPECT_TRUE(result.find("test") != std::string::npos);
    EXPECT_TRUE(result.find("replacement") != std::string::npos);
}