# RegularExpressions.Transformer C++

C++ implementation of Platform.RegularExpressions.Transformer library.

## Features

- **Text Transformation**: Apply sequences of regular expression substitutions to text
- **File Transformation**: Transform files and directories using regex rules
- **Rule-based Processing**: Configure multiple substitution rules with repeat counts
- **Cross-platform**: Uses standard C++17 and CMake for portability

## Requirements

- C++17 compatible compiler
- CMake 3.16 or later
- Google Test (automatically downloaded if not found)

## Building

```bash
mkdir build
cd build
cmake ..
make
```

## Running Tests

```bash
# From build directory
ctest
# or
./tests/RegularExpressionsTransformerTests
```

## Running Examples

```bash
# From build directory
./examples/BasicExample
./examples/FileExample
```

## Usage

### Basic Text Transformation

```cpp
#include "TextTransformer.h"
#include "SubstitutionRule.h"

using namespace Platform::RegularExpressions::Transformer;

// Create substitution rules
auto rule1 = std::make_shared<SubstitutionRule>("hello", "hi");
auto rule2 = std::make_shared<SubstitutionRule>("world", "universe");

std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule1, rule2};

// Create transformer
TextTransformer transformer(rules);

// Transform text
std::string result = transformer.transform("hello world");
// Result: "hi universe"
```

### File Transformation

```cpp
#include "FileTransformer.h"
#include "TextTransformer.h"
#include "SubstitutionRule.h"

using namespace Platform::RegularExpressions::Transformer;

// Create text transformer with rules
auto rule = std::make_shared<SubstitutionRule>(R"(\d+)", "[NUMBER]");
std::vector<std::shared_ptr<ISubstitutionRule>> rules = {rule};
auto textTransformer = std::make_shared<TextTransformer>(rules);

// Create file transformer
FileTransformer fileTransformer(textTransformer, ".txt", ".transformed");

// Transform file or directory
fileTransformer.transform("input.txt", "output.transformed");
fileTransformer.transform("source_folder/", "target_folder/");
```

### Advanced Rules

```cpp
// Rule with maximum repeat count
auto rule = std::make_shared<SubstitutionRule>("a+", "X", 3);

// Rule with custom regex options
auto complexRule = std::make_shared<SubstitutionRule>(
    R"((?i)hello)",  // Case-insensitive pattern
    "Hi",
    0,  // No repeat limit
    std::regex_constants::ECMAScript | std::regex_constants::icase
);

// Rule with path pattern for file filtering
rule->setPathPattern(R"(.*\.cpp$)");  // Only process .cpp files
```

## Architecture

- **ISubstitutionRule**: Interface for substitution rules
- **SubstitutionRule**: Implementation of substitution rules with regex patterns
- **ITransformer**: Base interface for transformers
- **ITextTransformer**: Interface for text transformation
- **TextTransformer**: Main text transformation implementation
- **TextSteppedTransformer**: Step-by-step text transformation for debugging
- **IFileTransformer**: Interface for file transformation
- **FileTransformer**: File and directory transformation implementation

## Installation

### Using CMake

```cmake
find_package(RegularExpressionsTransformer REQUIRED)
target_link_libraries(your_target Platform::RegularExpressions::RegularExpressionsTransformer)
```

### Manual Installation

```bash
mkdir build && cd build
cmake ..
make
sudo make install
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.