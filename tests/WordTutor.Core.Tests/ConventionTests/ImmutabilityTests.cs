using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace WordTutor.Core.Tests.ConventionTests
{
    public class ImmutabilityTests
    {
        [Theory]
        [MemberData(nameof(PropertiesOfImmutableTypesToTest))]
        public void PropertiesOfImmutableTypesShouldHaveImmutableTypes(PropertyInfo property)
        {
            property.PropertyType.IsImmutableType()
                .Should().BeTrue($"property {property.Name} should be declared as an immutable type");
        }

        public static IEnumerable<object[]> PropertiesOfImmutableTypesToTest()
            => from t in FindDeclaredImmutableTypes()
               from p in t.GetProperties()
               select new object[] { p };

        private static IEnumerable<Type> FindDeclaredImmutableTypes()
            => from t in typeof(WordTutorApplication).Assembly.GetTypes()
               where t.IsImmutableType()
               select t;
    }
}
