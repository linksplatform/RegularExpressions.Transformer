using System;
using Xunit;

namespace Platform.RegularExpressions.Transformer.Tests
{
    /// <summary>
    /// <para>
    /// Represents the regex engine tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public class RegexEngineTests
    {
        /// <summary>
        /// <para>
        /// Tests that system regex engine creates patterns correctly.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void SystemRegexEngine_CreatePattern_Test()
        {
            var engine = new SystemRegexEngine();
            var pattern = engine.CreatePattern(@"hello\s+world");
            
            Assert.NotNull(pattern);
            Assert.Equal(@"hello\s+world", pattern.Pattern);
            Assert.Equal("System.Text.RegularExpressions", engine.Name);
            Assert.True(pattern.IsMatch("hello world"));
            Assert.True(pattern.IsMatch("hello   world"));
            Assert.False(pattern.IsMatch("helloworld"));
        }

        /// <summary>
        /// <para>
        /// Tests that PCRE regex engine creates patterns correctly.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void PcreRegexEngine_CreatePattern_Test()
        {
            var engine = new PcreRegexEngine();
            var pattern = engine.CreatePattern(@"hello\s+world");
            
            Assert.NotNull(pattern);
            Assert.Equal(@"hello\s+world", pattern.Pattern);
            Assert.Equal("PCRE2", engine.Name);
            Assert.True(pattern.IsMatch("hello world"));
            Assert.True(pattern.IsMatch("hello   world"));
            Assert.False(pattern.IsMatch("helloworld"));
        }

        /// <summary>
        /// <para>
        /// Tests that system regex engine creates patterns with timeout correctly.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void SystemRegexEngine_CreatePatternWithTimeout_Test()
        {
            var engine = new SystemRegexEngine();
            var timeout = TimeSpan.FromSeconds(30);
            var pattern = engine.CreatePattern(@"test", timeout);
            
            Assert.NotNull(pattern);
            Assert.Equal(@"test", pattern.Pattern);
            Assert.Equal(timeout, pattern.MatchTimeout);
        }

        /// <summary>
        /// <para>
        /// Tests that PCRE regex engine creates patterns with timeout correctly.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void PcreRegexEngine_CreatePatternWithTimeout_Test()
        {
            var engine = new PcreRegexEngine();
            var timeout = TimeSpan.FromSeconds(30);
            var pattern = engine.CreatePattern(@"test", timeout);
            
            Assert.NotNull(pattern);
            Assert.Equal(@"test", pattern.Pattern);
            Assert.Equal(timeout, pattern.MatchTimeout);
        }

        /// <summary>
        /// <para>
        /// Tests that regex engine factory creates engines correctly.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void RegexEngineFactory_CreateEngine_Test()
        {
            var systemEngine = RegexEngineFactory.CreateEngine(RegexEngineType.SystemRegex);
            var pcreEngine = RegexEngineFactory.CreateEngine(RegexEngineType.PCRE2);
            
            Assert.IsType<SystemRegexEngine>(systemEngine);
            Assert.IsType<PcreRegexEngine>(pcreEngine);
            Assert.Equal("System.Text.RegularExpressions", systemEngine.Name);
            Assert.Equal("PCRE2", pcreEngine.Name);
        }

        /// <summary>
        /// <para>
        /// Tests that default engine setting works correctly.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void RegexEngineFactory_DefaultEngine_Test()
        {
            // Save original default
            var originalDefault = RegexEngineFactory.DefaultEngine;
            
            try
            {
                // Test setting PCRE2 as default
                RegexEngineFactory.SetDefaultEngine(RegexEngineType.PCRE2);
                Assert.IsType<PcreRegexEngine>(RegexEngineFactory.DefaultEngine);
                Assert.Equal(RegexEngineType.PCRE2, RegexEngineFactory.GetDefaultEngineType());
                
                // Test setting System as default
                RegexEngineFactory.SetDefaultEngine(RegexEngineType.SystemRegex);
                Assert.IsType<SystemRegexEngine>(RegexEngineFactory.DefaultEngine);
                Assert.Equal(RegexEngineType.SystemRegex, RegexEngineFactory.GetDefaultEngineType());
            }
            finally
            {
                // Restore original default
                RegexEngineFactory.DefaultEngine = originalDefault;
            }
        }

        /// <summary>
        /// <para>
        /// Tests that pattern replacement works correctly with both engines.
        /// </para>
        /// <para></para>
        /// </summary>
        [Theory]
        [InlineData(typeof(SystemRegexEngine))]
        [InlineData(typeof(PcreRegexEngine))]
        public void RegexPattern_Replace_Test(Type engineType)
        {
            var engine = (IRegexEngine)Activator.CreateInstance(engineType)!;
            var pattern = engine.CreatePattern(@"\b\d{4}\b");
            
            var input = "The year 2024 was amazing, unlike 2023.";
            var result = pattern.Replace(input, "XXXX");
            
            Assert.Equal("The year XXXX was amazing, unlike XXXX.", result);
        }

        /// <summary>
        /// <para>
        /// Tests that pattern WithTimeout works correctly.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void RegexPattern_WithTimeout_Test()
        {
            var engine = new SystemRegexEngine();
            var pattern = engine.CreatePattern(@"test");
            var newTimeout = TimeSpan.FromMinutes(2);
            
            var newPattern = pattern.WithTimeout(newTimeout);
            
            Assert.NotEqual(pattern.MatchTimeout, newPattern.MatchTimeout);
            Assert.Equal(newTimeout, newPattern.MatchTimeout);
            Assert.Equal(pattern.Pattern, newPattern.Pattern);
        }
    }
}