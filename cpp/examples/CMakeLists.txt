cmake_minimum_required(VERSION 3.16)

# Basic example
add_executable(BasicExample
    basic_example.cpp
)

target_link_libraries(BasicExample
    RegularExpressionsTransformer
)

# File transformation example
add_executable(FileExample
    file_example.cpp
)

target_link_libraries(FileExample
    RegularExpressionsTransformer
)