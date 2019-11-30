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
        [MemberData(nameof(TestCasesArePropertiesOfImmutableTypes))]
        public void PropertiesOfImmutableTypesShouldHaveImmutableTypes(PropertyInfo property)
        {
            property.PropertyType.IsImmutableType().Should().BeTrue(
                $"property {property.Name} should be declared as an immutable type");
        }

        [Theory]
        [MemberData(nameof(TestCasesArePropertiesOfImmutableTypes))]
        public void PropertiesOfImmutableTypesMustNotBeWritable(PropertyInfo property)
        {
            property.CanWrite.Should().BeFalse(
                $"property {property.Name} of immutable type {property.DeclaringType!.Name} should not be writable");
        }

        [Theory]
        [MemberData(nameof(TestCasesAreSubclassesOfImmutableTypes))]
        public void SubTypesOfImmutableTypesMustBeImmutable(Type type)
        {
            type.IsImmutableType().Should().BeTrue(
                $"Type {type.Name} should be marked [Immutable] because it descends from immutable type {type.BaseType.Name}.");
        }

        [Theory]
        [MemberData(nameof(TestCasesAreImmutableTypes))]
        public void ImmutableTypesShouldBeSealedOrAbstract(Type type)
        {
            type.Should().Match(t => t.IsAbstract || t.IsSealed);
        }

        public static IEnumerable<object[]> TestCasesAreImmutableTypes()
            => from t in FindDeclaredImmutableTypes()
               select new object[] { t };

        public static IEnumerable<object[]> TestCasesAreSubclassesOfImmutableTypes()
        {
            var immutableTypes = FindDeclaredImmutableTypes().ToHashSet();
            return from t in typeof(WordTutorApplication).Assembly.GetTypes()
                   where t.BaseType is Type && immutableTypes.Contains(t.BaseType)
                   select new object[] { t };
        }

        public static IEnumerable<object[]> TestCasesArePropertiesOfImmutableTypes()
            => from t in FindDeclaredImmutableTypes()
               from p in t.GetProperties()
               select new object[] { p };

        private static IEnumerable<Type> FindDeclaredImmutableTypes()
            => from t in typeof(WordTutorApplication).Assembly.GetTypes()
               where t.IsImmutableType()
               select t;
    }
}
