using FluentAssertions;

using Frank.Markdown;

using Xunit.Abstractions;

namespace Frank.Reflection.Tests
{
    public class TypeExtensionsTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public TypeExtensionsTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void GetDisplayName_ReturnsCorrectDisplayNameForSimpleType()
        {
            // Arrange
            Type type = typeof(int);

            // Act
            string displayName = type.GetDisplayName();

            // Assert
            Assert.Equal("Integer", displayName);
        }

        [Fact]
        public void GetDisplayName_ReturnsCorrectDisplayNameForGenericType()
        {
            // Arrange
            Type type = typeof(Dictionary<string, object>);

            // Act
            string displayName = type.GetDisplayName();

            // Assert
            Assert.Equal("DictionaryOfStringAndObject", displayName);
        }

        [Fact]
        public void GetFullDisplayName_ReturnsCorrectFullDisplayNameForSimpleType()
        {
            // Arrange
            Type type = typeof(int);

            // Act
            string fullDisplayName = type.GetFullDisplayName();

            // Assert
            Assert.Equal("System.Integer", fullDisplayName);
        }

        [Fact]
        public void GetFullDisplayName_ReturnsCorrectFullDisplayNameForGenericType()
        {
            // Arrange
            Type type = typeof(Dictionary<string, object>);

            // Act
            string fullDisplayName = type.GetFullDisplayName();

            // Assert
            Assert.Equal("System.Collections.Generic.DictionaryOfStringAndObject", fullDisplayName);
        }

        [Fact]
        public void GetFriendlyName_ReturnsCorrectFriendlyNameForSimpleType()
        {
            // Arrange
            Type type = typeof(int);

            // Act
            string friendlyName = type.GetFriendlyName();

            // Assert
            Assert.Equal("Integer", friendlyName);
        }

        [Fact]
        public void GetFriendlyName_ReturnsCorrectFriendlyNameForGenericType()
        {
            // Arrange
            Type type = typeof(Dictionary<string, object>);

            // Act
            string friendlyName = type.GetFriendlyName();

            // Assert
            Assert.Equal("Dictionary<string, object>", friendlyName, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void GetFullFriendlyName_ReturnsCorrectFullFriendlyNameForSimpleType()
        {
            // Arrange
            Type type = typeof(int);

            // Act
            string fullFriendlyName = type.GetFullFriendlyName();

            // Assert
            Assert.Equal("System.Integer", fullFriendlyName);
        }

        [Fact]
        public void GetFullFriendlyName_ReturnsCorrectFullFriendlyNameForGenericType()
        {
            // Arrange
            Type type = typeof(Dictionary<string, object>);

            // Act
            string fullFriendlyName = type.GetFullFriendlyName();

            // Assert
            Assert.Equal("System.Collections.Generic.Dictionary<string, object>", fullFriendlyName, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void GetMarkdownTest()
        {
            // Arrange
            Type type = typeof(MarkdownDocument);

            // Act
            // var markdown = type.GetDocumentation();

            // Assert
            // _outputHelper.WriteLine(markdown.ToString());
        }
    }
}