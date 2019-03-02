using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Niche.Common.Tests
{
    public class WarningResultTests
    {
        public class Constructor : WarningResultTests
        {
            private readonly IEnumerable<ValidationMetadata> _emptyData
                = new List<ValidationMetadata>();

            [Fact]
            public void GivenMessage_InitializesProperty()
            {
                var message = "Expected warning message";
                var warning = new WarningResult(message, _emptyData);
                warning.Message.Should().Be(message);
            }

            [Fact]
            public void GivenData_InitializesProperty()
            {
                var message = "Expected warning message";
                var warning = new WarningResult(message, _emptyData);
                warning.Metadata.Should().Equal(_emptyData);
            }

            [Fact]
            public void GivenNullMessage_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new WarningResult(null, _emptyData));
                exception.ParamName.Should().Be("message");
            }

            [Fact]
            public void GivenNullData_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new WarningResult("message", null));
                exception.ParamName.Should().Be("metadata");
            }
        }
    }
}
