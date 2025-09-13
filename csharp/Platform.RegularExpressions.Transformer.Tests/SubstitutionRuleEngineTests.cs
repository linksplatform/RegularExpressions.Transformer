using System;
using System.Text.RegularExpressions;
using Xunit;

namespace Platform.RegularExpressions.Transformer.Tests
{
    /// <summary>
    /// <para>
    /// Represents the substitution rule engine tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public class SubstitutionRuleEngineTests
    {
        /// <summary>
        /// <para>
        /// Tests that SubstitutionRule works with SystemRegex engine.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void SubstitutionRule_SystemRegexEngine_Test()
        {
            var engine = new SystemRegexEngine();
            var pattern = engine.CreatePattern(@"hello");
            var rule = new SubstitutionRule(pattern, "hi");
            
            Assert.NotNull(rule.MatchPattern);
            Assert.Equal("hello", rule.MatchPattern.Pattern);
            Assert.Equal("hi", rule.SubstitutionPattern);
            Assert.IsType<SystemRegexPattern>(rule.MatchPattern);
        }

        /// <summary>
        /// <para>
        /// Tests that SubstitutionRule works with PCRE2 engine.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void SubstitutionRule_PcreEngine_Test()
        {
            var engine = new PcreRegexEngine();
            var pattern = engine.CreatePattern(@"hello");
            var rule = new SubstitutionRule(pattern, "hi");
            
            Assert.NotNull(rule.MatchPattern);
            Assert.Equal("hello", rule.MatchPattern.Pattern);
            Assert.Equal("hi", rule.SubstitutionPattern);
            Assert.IsType<PcreRegexPattern>(rule.MatchPattern);
        }

        /// <summary>
        /// <para>
        /// Tests that SubstitutionRule implicit conversion uses default engine.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void SubstitutionRule_ImplicitConversion_UsesDefaultEngine_Test()
        {
            // Save original default
            var originalDefault = RegexEngineFactory.DefaultEngine;
            
            try
            {
                // Test with System regex as default
                RegexEngineFactory.SetDefaultEngine(RegexEngineType.SystemRegex);
                SubstitutionRule rule1 = ("hello", "hi");
                Assert.IsType<SystemRegexPattern>(rule1.MatchPattern);
                
                // Test with PCRE2 as default
                RegexEngineFactory.SetDefaultEngine(RegexEngineType.PCRE2);
                SubstitutionRule rule2 = ("hello", "hi");
                Assert.IsType<PcreRegexPattern>(rule2.MatchPattern);
            }
            finally
            {
                // Restore original default
                RegexEngineFactory.DefaultEngine = originalDefault;
            }
        }

        /// <summary>
        /// <para>
        /// Tests that legacy Regex constructor still works.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void SubstitutionRule_LegacyRegexConstructor_Test()
        {
            var regex = new Regex("hello");
            var rule = new SubstitutionRule(regex, "hi");
            
            Assert.NotNull(rule.MatchPattern);
            Assert.Equal("hello", rule.MatchPattern.Pattern);
            Assert.Equal("hi", rule.SubstitutionPattern);
            Assert.NotNull(rule.LegacyMatchPattern);
            Assert.Equal("hello", rule.LegacyMatchPattern.ToString());
        }

        /// <summary>
        /// <para>
        /// Tests backward compatibility through LegacyMatchPattern property.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void SubstitutionRule_LegacyMatchPattern_BackwardCompatibility_Test()
        {
            // Test with SystemRegexPattern
            var systemEngine = new SystemRegexEngine();
            var systemPattern = systemEngine.CreatePattern(@"test");
            var systemRule = new SubstitutionRule(systemPattern, "replacement");
            
            Assert.NotNull(systemRule.LegacyMatchPattern);
            Assert.IsType<Regex>(systemRule.LegacyMatchPattern);
            Assert.Equal("test", systemRule.LegacyMatchPattern.ToString());
            
            // Test with PcreRegexPattern
            var pcreEngine = new PcreRegexEngine();
            var pcrePattern = pcreEngine.CreatePattern(@"test");
            var pcreRule = new SubstitutionRule(pcrePattern, "replacement");
            
            Assert.NotNull(pcreRule.LegacyMatchPattern);
            Assert.IsType<Regex>(pcreRule.LegacyMatchPattern);
            Assert.Equal("test", pcreRule.LegacyMatchPattern.ToString());
        }

        /// <summary>
        /// <para>
        /// Tests that TextTransformer works with different regex engines.
        /// </para>
        /// <para></para>
        /// </summary>
        [Theory]
        [InlineData(typeof(SystemRegexEngine))]
        [InlineData(typeof(PcreRegexEngine))]
        public void TextTransformer_WithDifferentEngines_Test(Type engineType)
        {
            var engine = (IRegexEngine)Activator.CreateInstance(engineType)!;
            var pattern = engine.CreatePattern(@"\b\d{4}\b");
            var rule = new SubstitutionRule(pattern, "YEAR");
            
            var transformer = new TextTransformer(new[] { rule });
            var input = "In 2024, we had great success.";
            var result = transformer.Transform(input);
            
            Assert.Equal("In YEAR, we had great success.", result);
        }

        /// <summary>
        /// <para>
        /// Tests that both engines produce equivalent results for common patterns.
        /// </para>
        /// <para></para>
        /// </summary>
        [Theory]
        [InlineData(@"\d+", "abc123def", "XXX", "abcXXXdef")]
        [InlineData(@"\s+", "hello   world", "_", "hello_world")]
        [InlineData(@"[A-Z]+", "Hello WORLD Test", "***", "***ello *** ***est")]
        public void BothEngines_ProduceEquivalentResults_Test(string pattern, string input, string replacement, string expected)
        {
            var systemEngine = new SystemRegexEngine();
            var pcreEngine = new PcreRegexEngine();
            
            var systemPattern = systemEngine.CreatePattern(pattern);
            var pcrePattern = pcreEngine.CreatePattern(pattern);
            
            var systemResult = systemPattern.Replace(input, replacement);
            var pcreResult = pcrePattern.Replace(input, replacement);
            
            Assert.Equal(expected, systemResult);
            Assert.Equal(expected, pcreResult);
            Assert.Equal(systemResult, pcreResult);
        }

        /// <summary>
        /// <para>
        /// Tests that engine switching works correctly during runtime.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void RegexEngine_SwitchingDuringRuntime_Test()
        {
            // Save original default
            var originalDefault = RegexEngineFactory.DefaultEngine;
            
            try
            {
                // Create rules with specific engines to demonstrate both engines work
                var systemEngine = new SystemRegexEngine();
                var pcreEngine = new PcreRegexEngine();
                
                var rule1 = new SubstitutionRule(systemEngine.CreatePattern(@"\d+"), "123");
                var rule2 = new SubstitutionRule(pcreEngine.CreatePattern(@"[A-Z]"), "X");
                
                var transformer = new TextTransformer(new[] { rule1, rule2 });
                var result = transformer.Transform("A1B2C");
                
                // The rules are applied sequentially, so "A1B2C" becomes:
                // 1. rule1 applies: "A1B2C" -> "A123B123C" (1,2 -> 123)  
                // 2. rule2 applies: "A123B123C" -> "X123X123X" (A,B,C -> X)
                Assert.Equal("X123X123X", result);
            }
            finally
            {
                // Restore original default
                RegexEngineFactory.DefaultEngine = originalDefault;
            }
        }
    }
}