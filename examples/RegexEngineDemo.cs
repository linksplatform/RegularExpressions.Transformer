using System;
using Platform.RegularExpressions.Transformer;

namespace Platform.RegularExpressions.Transformer.Examples
{
    /// <summary>
    /// Demonstrates how to use the new PCRE2 regex engine support alongside System.Text.RegularExpressions
    /// </summary>
    public class RegexEngineDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== PCRE2 Regex Engine Support Demo ===");
            Console.WriteLine();

            // 1. Default behavior (uses System.Text.RegularExpressions)
            Console.WriteLine("1. Using default System.Text.RegularExpressions engine:");
            DemonstrateSystemRegexEngine();
            Console.WriteLine();

            // 2. Switching to PCRE2 engine
            Console.WriteLine("2. Switching to PCRE2 engine:");
            DemonstratePcreEngine();
            Console.WriteLine();

            // 3. Using both engines in same transformation
            Console.WriteLine("3. Using both engines in the same transformation:");
            DemonstrateMixedEngines();
            Console.WriteLine();

            // 4. Engine switching at runtime
            Console.WriteLine("4. Runtime engine switching:");
            DemonstrateRuntimeSwitching();
        }

        private static void DemonstrateSystemRegexEngine()
        {
            // Default engine is System.Text.RegularExpressions
            SubstitutionRule rule = (@"\b\d{4}\b", "YEAR");
            var transformer = new TextTransformer(new[] { rule });
            
            string input = "The years 2023 and 2024 were eventful.";
            string result = transformer.Transform(input);
            
            Console.WriteLine($"Input:  {input}");
            Console.WriteLine($"Output: {result}");
            Console.WriteLine($"Engine: {RegexEngineFactory.DefaultEngine.Name}");
        }

        private static void DemonstratePcreEngine()
        {
            // Switch to PCRE2 engine
            RegexEngineFactory.SetDefaultEngine(RegexEngineType.PCRE2);
            
            SubstitutionRule rule = (@"\b[A-Z]{3}\b", "***");
            var transformer = new TextTransformer(new[] { rule });
            
            string input = "The CEO met with CTO and CFO.";
            string result = transformer.Transform(input);
            
            Console.WriteLine($"Input:  {input}");
            Console.WriteLine($"Output: {result}");
            Console.WriteLine($"Engine: {RegexEngineFactory.DefaultEngine.Name}");
            
            // Reset to default
            RegexEngineFactory.SetDefaultEngine(RegexEngineType.SystemRegex);
        }

        private static void DemonstrateMixedEngines()
        {
            // Create engines explicitly
            var systemEngine = new SystemRegexEngine();
            var pcreEngine = new PcreRegexEngine();
            
            // Create rules using different engines
            var rule1 = new SubstitutionRule(systemEngine.CreatePattern(@"\d+"), "###");
            var rule2 = new SubstitutionRule(pcreEngine.CreatePattern(@"[A-Z]+"), "XXX");
            
            var transformer = new TextTransformer(new[] { rule1, rule2 });
            
            string input = "Order 123 for PRODUCT ABC";
            string result = transformer.Transform(input);
            
            Console.WriteLine($"Input:  {input}");
            Console.WriteLine($"Output: {result}");
            Console.WriteLine($"Rule 1 Engine: {rule1.MatchPattern.GetType().Name} (System.Text.RegularExpressions)");
            Console.WriteLine($"Rule 2 Engine: {rule2.MatchPattern.GetType().Name} (PCRE2)");
        }

        private static void DemonstrateRuntimeSwitching()
        {
            Console.WriteLine("Creating rules with different engines at runtime...");
            
            // Start with System engine
            RegexEngineFactory.SetDefaultEngine(RegexEngineType.SystemRegex);
            SubstitutionRule rule1 = (@"hello", "hi");
            Console.WriteLine($"Rule1 created with: {RegexEngineFactory.DefaultEngine.Name}");
            
            // Switch to PCRE2 engine
            RegexEngineFactory.SetDefaultEngine(RegexEngineType.PCRE2);
            SubstitutionRule rule2 = (@"world", "universe");
            Console.WriteLine($"Rule2 created with: {RegexEngineFactory.DefaultEngine.Name}");
            
            var transformer = new TextTransformer(new[] { rule1, rule2 });
            
            string input = "hello world";
            string result = transformer.Transform(input);
            
            Console.WriteLine($"Input:  {input}");
            Console.WriteLine($"Output: {result}");
            
            // Reset to default
            RegexEngineFactory.SetDefaultEngine(RegexEngineType.SystemRegex);
        }
    }
}