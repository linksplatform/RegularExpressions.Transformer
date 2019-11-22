import retranslator, cs2cpp

retranslator.TranslatorCLI(args=["test.cs"], translator=cs2cpp.CSharpToCpp, extension=".cpp",
    useRegex=1)