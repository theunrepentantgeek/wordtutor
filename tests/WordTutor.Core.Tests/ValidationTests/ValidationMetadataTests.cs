using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Niche.Common.Tests
{
    public class ValidationMetadataTests
    {
        private readonly string _name = "Name";

        private readonly object _value = 42;

        public class Constructor : ValidationMetadataTests
        {
            [Fact]
            public void GivenNullName_ThrowsExpectedException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new ValidationMetadata(null, _value));
                exception.ParamName.Should().Be("name");
            }

            [Fact]
            public void GivenBlankName_ThrowsExpectedException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new ValidationMetadata(string.Empty, _value));
                exception.ParamName.Should().Be("name");
            }

            [Fact]
            public void GivenName_SetsProperty()
            {
                var m = new ValidationMetadata(_name, _value);
                m.Name.Should().Be(_name);
            }

            [Fact]
            public void GivenValue_SetsProperty()
            {
                var m = new ValidationMetadata(_name, _value);
                m.Value.Should().Be(_value);
            }
        }

        public class EqualsValidationMetadata : ValidationMetadataTests
        {
            private readonly ValidationMetadata _metadata;

            public EqualsValidationMetadata()
            {
                _metadata = new ValidationMetadata(_name, _value);               
            }

            [Fact]
            public void GivenNull_ReturnsFalse()
            {
                ValidationMetadata vm = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                _metadata.Equals(vm).Should().BeFalse();
            }

            [Fact]
            public void GivenSelf_ReturnsTrue()
            {
                _metadata.Equals(_metadata).Should().BeTrue();
            }

            [Fact]
            public void GivenSame_ReturnsTrue()
            {
                var same = new ValidationMetadata(_name, _value);
                _metadata.Equals(same).Should().BeTrue();
            }

            [Fact]
            public void WhenDiffersByName_ReturnsFalse()
            {
                var differsByName = new ValidationMetadata("other", _value);
                _metadata.Equals(differsByName).Should().BeFalse();
            }

            [Fact]
            public void WhenDiffersByValue_ReturnsFalse()
            {
                var differsByValue = new ValidationMetadata(_name, 17);
                _metadata.Equals(differsByValue).Should().BeFalse();
            }
        }

        public class EqualsObject : ValidationMetadataTests
        {
            private readonly ValidationMetadata _metadata;

            public EqualsObject()
            {
                _metadata = new ValidationMetadata(_name, _value);
            }

            [Fact]
            public void GivenNull_ReturnsFalse()
            {
                object vm = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                _metadata.Equals(vm).Should().BeFalse();
            }

            [Fact]
            public void GivenSelf_ReturnsTrue()
            {
                object self = _metadata;
                _metadata.Equals(self).Should().BeTrue();
            }
        }
    }
}
