using System;
using System.Collections.Generic;

using FluentAssertions;
using Xunit;

namespace WordTutor.Core.Common.Tests
{
    public class ErrorResultTests
    {
        public class Constructor : ErrorResultTests
        {
            private readonly IEnumerable<ValidationMetadata> _emptyData
                = new List<ValidationMetadata>();

            [Fact]
            public void GivenMessage_InitializesProperty()
            {
                var message = "Expected error message";
                var error = new ErrorResult(message, _emptyData);
                error.Message.Should().Be(message);
            }

            [Fact]
            public void GivenEmptyData_InitializesProperty()
            {
                var message = "Expected error message";
                var error = new ErrorResult(message, _emptyData);
                error.Metadata.Should().Equal(_emptyData);
            }

            [Fact]
            public void GivenNullMessage_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new ErrorResult(null, _emptyData));
                exception.ParamName.Should().Be("message");
            }

            [Fact]
            public void GivenNullData_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new ErrorResult("message", null));
                exception.ParamName.Should().Be("metadata");
            }
        }
    }
}
