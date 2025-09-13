cmake_minimum_required(VERSION 3.16)

# Find Google Test
find_package(GTest)

if(NOT GTest_FOUND)
    # If GTest is not found, try to build it from source
    include(FetchContent)
    FetchContent_Declare(
        googletest
        GIT_REPOSITORY https://github.com/google/googletest.git
        GIT_TAG        release-1.12.1
    )
    FetchContent_MakeAvailable(googletest)
    
    # For compatibility with older CMake versions
    if(NOT TARGET GTest::GTest)
        add_library(GTest::GTest ALIAS gtest)
        add_library(GTest::Main ALIAS gtest_main)
    endif()
endif()

# Create test executable
add_executable(RegularExpressionsTransformerTests
    SubstitutionRuleTests.cpp
    TextTransformerTests.cpp
    FileTransformerTests.cpp
)

target_link_libraries(RegularExpressionsTransformerTests
    RegularExpressionsTransformer
    GTest::GTest
    GTest::Main
)

# Add the test to CTest
add_test(NAME RegularExpressionsTransformerTests 
         COMMAND RegularExpressionsTransformerTests)