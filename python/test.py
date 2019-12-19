import retranslator
import cs2cpp

retranslator.TranslatorCLI(args=["cs", "cpp"], original_extension=".cs", translator=cs2cpp.CSharpToCpp,
                           extension=".cpp", useRegex=1, exludeExtra=True)
