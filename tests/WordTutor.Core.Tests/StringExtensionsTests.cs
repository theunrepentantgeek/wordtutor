using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace WordTutor.Core.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("DemoScreen", "Demo")]
        [InlineData("Demo", "")]
        [InlineData(null, "")]
        [InlineData("", "")]
        public void RemoveSuffix_GivenString_ReturnsExpectation(string value, string expected)
        {
            value.RemoveSuffix().Should().Be(expected);
        }

        [Theory]
        [InlineData("DemoScreen", "View", "DemoView")]
        [InlineData("DemoViewModel", "Connector", "DemoViewConnector")]
        [InlineData("Demo", "Model", "Model")]
        [InlineData(null, "Rule", "Rule")]
        [InlineData("", "Rule", "Rule")]
        public void ReplaceSuffix_GivenString_ReturnsExpectation(string value, string suffix, string expected)
        {
            value.ReplaceSuffix(suffix).Should().Be(expected);
        }
    }
}
