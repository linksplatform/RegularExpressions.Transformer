cmake_minimum_required(VERSION 3.16)

project(RegularExpressionsTransformer 
        VERSION 1.0.0 
        DESCRIPTION "Platform Regular Expressions Transformer C++ Library"
        LANGUAGES CXX)

# Set C++ standard
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

# Add compiler-specific options
if(MSVC)
    add_compile_options(/W4 /permissive-)
else()
    add_compile_options(-Wall -Wextra -Wpedantic)
endif()

# Define the library target
add_library(RegularExpressionsTransformer
    src/TextSteppedTransformer.cpp
    src/FileTransformer.cpp
)

# Set target properties
set_target_properties(RegularExpressionsTransformer PROPERTIES
    VERSION ${PROJECT_VERSION}
    SOVERSION ${PROJECT_VERSION_MAJOR}
    PUBLIC_HEADER "include/ISubstitutionRule.h;include/SubstitutionRule.h;include/ITransformer.h;include/ITextTransformer.h;include/TextSteppedTransformer.h;include/TextTransformer.h;include/IFileTransformer.h;include/FileTransformer.h"
)

# Include directories
target_include_directories(RegularExpressionsTransformer
    PUBLIC 
        $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
        $<INSTALL_INTERFACE:include>
    PRIVATE
        ${CMAKE_CURRENT_SOURCE_DIR}/src
)

# Find and link required libraries
find_package(Threads REQUIRED)
target_link_libraries(RegularExpressionsTransformer 
    PUBLIC 
        Threads::Threads
)

# Enable testing
enable_testing()

# Add tests subdirectory if it exists
if(EXISTS ${CMAKE_CURRENT_SOURCE_DIR}/tests)
    add_subdirectory(tests)
endif()

# Add examples subdirectory if it exists
if(EXISTS ${CMAKE_CURRENT_SOURCE_DIR}/examples)
    add_subdirectory(examples)
endif()

# Install targets
include(GNUInstallDirs)

install(TARGETS RegularExpressionsTransformer
    EXPORT RegularExpressionsTransformerTargets
    LIBRARY DESTINATION ${CMAKE_INSTALL_LIBDIR}
    ARCHIVE DESTINATION ${CMAKE_INSTALL_LIBDIR}
    RUNTIME DESTINATION ${CMAKE_INSTALL_BINDIR}
    PUBLIC_HEADER DESTINATION ${CMAKE_INSTALL_INCLUDEDIR}/Platform/RegularExpressions/Transformer
)

# Install the export set
install(EXPORT RegularExpressionsTransformerTargets
    FILE RegularExpressionsTransformerTargets.cmake
    NAMESPACE Platform::RegularExpressions::
    DESTINATION ${CMAKE_INSTALL_LIBDIR}/cmake/RegularExpressionsTransformer
)

# Create config files
include(CMakePackageConfigHelpers)

configure_package_config_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/cmake/RegularExpressionsTransformerConfig.cmake.in
    ${CMAKE_CURRENT_BINARY_DIR}/RegularExpressionsTransformerConfig.cmake
    INSTALL_DESTINATION ${CMAKE_INSTALL_LIBDIR}/cmake/RegularExpressionsTransformer
)

write_basic_package_version_file(
    ${CMAKE_CURRENT_BINARY_DIR}/RegularExpressionsTransformerConfigVersion.cmake
    VERSION ${PROJECT_VERSION}
    COMPATIBILITY AnyNewerVersion
)

install(FILES
    ${CMAKE_CURRENT_BINARY_DIR}/RegularExpressionsTransformerConfig.cmake
    ${CMAKE_CURRENT_BINARY_DIR}/RegularExpressionsTransformerConfigVersion.cmake
    DESTINATION ${CMAKE_INSTALL_LIBDIR}/cmake/RegularExpressionsTransformer
)