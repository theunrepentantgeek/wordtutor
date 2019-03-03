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

            private readonly string _message = "Expected error message";

            [Fact]
            public void GivenMessage_InitializesProperty()
            {
                var error = new ErrorResult(_message, _emptyData);
                error.Message.Should().Be(_message);
            }

            [Fact]
            public void GivenEmptyData_InitializesProperty()
            {
                var error = new ErrorResult(_message, _emptyData);
                error.Metadata.Should().Equal(_emptyData);
            }

            [Fact]
            public void GivenData_InitializesProperty()
            {
                var answer = new ValidationMetadata("answer", 42);
                var metadata = new List<ValidationMetadata> { answer };
                var error = new ErrorResult(_message, metadata);
                error.Metadata.Should().Contain(
                    kvp => kvp.Key == answer.Name && kvp.Value == answer.Value);
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
