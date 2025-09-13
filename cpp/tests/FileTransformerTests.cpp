#include <gtest/gtest.h>
#include "../include/FileTransformer.h"
#include "../include/TextTransformer.h"
#include "../include/SubstitutionRule.h"
#include <filesystem>
#include <fstream>
#include <memory>

using namespace Platform::RegularExpressions::Transformer;

class FileTransformerTests : public ::testing::Test {
protected:
    std::string tempDir;
    
    void SetUp() override {
        tempDir = std::filesystem::temp_directory_path() / "transformer_tests";
        std::filesystem::create_directories(tempDir);
    }
    
    void TearDown() override {
        if (std::filesystem::exists(tempDir)) {
            std::filesystem::remove_all(tempDir);
        }
    }
    
    void createTestFile(const std::string& path, const std::string& content) {
        std::ofstream file(path);
        file << content;
        file.close();
    }
    
    std::string readTestFile(const std::string& path) {
        std::ifstream file(path);
        std::string content((std::istreambuf_iterator<char>(file)),
                           std::istreambuf_iterator<char>());
        return content;
    }
};

TEST_F(FileTransformerTests, TransformSingleFile) {
    // Create transformer
    auto rule = std::make_shared<SubstitutionRule>("hello", "hi");
    std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule};
    auto textTransformer = std::make_shared<TextTransformer>(rules);
    
    FileTransformer fileTransformer(textTransformer, ".txt", ".out");
    
    // Create test file
    std::string sourcePath = tempDir + "/test.txt";
    std::string targetPath = tempDir + "/test.out";
    createTestFile(sourcePath, "hello world");
    
    // Transform
    fileTransformer.transform(sourcePath, targetPath);
    
    // Verify result
    EXPECT_TRUE(std::filesystem::exists(targetPath));
    EXPECT_EQ("hi world", readTestFile(targetPath));
}

TEST_F(FileTransformerTests, TransformFileToDirectory) {
    // Create transformer
    auto rule = std::make_shared<SubstitutionRule>("test", "demo");
    std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule};
    auto textTransformer = std::make_shared<TextTransformer>(rules);
    
    FileTransformer fileTransformer(textTransformer, ".txt", ".out");
    
    // Create test file and target directory
    std::string sourcePath = tempDir + "/source.txt";
    std::string targetDir = tempDir + "/output/";
    std::filesystem::create_directories(targetDir);
    createTestFile(sourcePath, "test content");
    
    // Transform
    fileTransformer.transform(sourcePath, targetDir);
    
    // Verify result
    std::string expectedTarget = targetDir + "source.out";
    EXPECT_TRUE(std::filesystem::exists(expectedTarget));
    EXPECT_EQ("demo content", readTestFile(expectedTarget));
}

TEST_F(FileTransformerTests, TransformDirectory) {
    // Create transformer
    auto rule = std::make_shared<SubstitutionRule>("old", "new");
    std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule};
    auto textTransformer = std::make_shared<TextTransformer>(rules);
    
    FileTransformer fileTransformer(textTransformer, ".txt", ".out");
    
    // Create source directory with files
    std::string sourceDir = tempDir + "/source/";
    std::string targetDir = tempDir + "/target/";
    std::filesystem::create_directories(sourceDir);
    
    createTestFile(sourceDir + "file1.txt", "old content 1");
    createTestFile(sourceDir + "file2.txt", "old content 2");
    createTestFile(sourceDir + "ignore.log", "old content 3"); // Should be ignored
    
    // Transform
    fileTransformer.transform(sourceDir, targetDir);
    
    // Verify results
    EXPECT_TRUE(std::filesystem::exists(targetDir + "file1.out"));
    EXPECT_TRUE(std::filesystem::exists(targetDir + "file2.out"));
    EXPECT_FALSE(std::filesystem::exists(targetDir + "ignore.log"));
    
    EXPECT_EQ("new content 1", readTestFile(targetDir + "file1.out"));
    EXPECT_EQ("new content 2", readTestFile(targetDir + "file2.out"));
}

TEST_F(FileTransformerTests, NonExistentSourceFile) {
    auto rule = std::make_shared<SubstitutionRule>("a", "b");
    std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule};
    auto textTransformer = std::make_shared<TextTransformer>(rules);
    
    FileTransformer fileTransformer(textTransformer, ".txt", ".out");
    
    std::string sourcePath = tempDir + "/nonexistent.txt";
    std::string targetPath = tempDir + "/output.out";
    
    EXPECT_THROW(fileTransformer.transform(sourcePath, targetPath), std::runtime_error);
}

TEST_F(FileTransformerTests, SkipIfTargetNewer) {
    // Create transformer
    auto rule = std::make_shared<SubstitutionRule>("old", "new");
    std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule};
    auto textTransformer = std::make_shared<TextTransformer>(rules);
    
    FileTransformer fileTransformer(textTransformer, ".txt", ".out");
    
    // Create source file
    std::string sourcePath = tempDir + "/source.txt";
    std::string targetPath = tempDir + "/target.out";
    createTestFile(sourcePath, "old content");
    
    // Create target file with newer timestamp
    createTestFile(targetPath, "existing content");
    
    // Make target newer by modifying its timestamp
    auto now = std::filesystem::file_time_type::clock::now();
    std::filesystem::last_write_time(targetPath, now + std::chrono::seconds(1));
    
    // Transform (should skip)
    fileTransformer.transform(sourcePath, targetPath);
    
    // Verify target wasn't changed
    EXPECT_EQ("existing content", readTestFile(targetPath));
}