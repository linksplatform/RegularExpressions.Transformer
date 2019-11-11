import retranslator
import cs2cpp


tcli = retranslator.TranslatorCLI(["test.cs"], cs2cpp.CSharpToCppTranslator, ".html")