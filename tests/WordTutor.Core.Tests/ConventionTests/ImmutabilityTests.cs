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
        [MemberData(nameof(FindPropertiesOfImmutableTypes))]
        public void PropertiesOfImmutableTypesShouldHaveImmutableTypes(PropertyInfo property)
        {
            property.PropertyType.IsImmutableType().Should().BeTrue(
                $"property {property.Name} should be declared as an immutable type");
        }

        [Theory]
        [MemberData(nameof(FindPropertiesOfImmutableTypes))]
        public void PropertiesOfImmutableTypesMustNotBeWritable(PropertyInfo property)
        {
            property.CanWrite.Should().BeFalse(
                $"property {property.Name} of immutable type {property.DeclaringType!.Name} should not be writable");
        }

        public static IEnumerable<object[]> FindPropertiesOfImmutableTypes()
            => from t in GetForImmutableTypes()
               from p in t.GetProperties()
               select new object[] { p };

        [Theory]
        [MemberData(nameof(FindSubclassesOfImmutableTypes))]
        public void SubTypesOfImmutableTypesMustBeImmutable(Type type)
        {
            type.IsImmutableType().Should().BeTrue(
                $"Type {type.Name} should be marked [Immutable] because it descends from immutable type {type.BaseType.Name}.");
        }

        public static IEnumerable<object[]> FindSubclassesOfImmutableTypes()
        {
            var immutableTypes = GetForImmutableTypes().ToHashSet();
            return from t in typeof(WordTutorApplication).Assembly.GetTypes()
                   where t.BaseType is Type && immutableTypes.Contains(t.BaseType)
                   select new object[] { t };
        }

        [Theory]
        [MemberData(nameof(FindImmutableTypes))]
        public void ImmutableTypesShouldBeSealedOrAbstract(Type type)
        {
            type.Should().Match(t => t.IsAbstract || t.IsSealed);
        }

        public static IEnumerable<object[]> FindImmutableTypes()
            => from t in GetForImmutableTypes()
               select new object[] { t };

        private static IEnumerable<Type> GetForImmutableTypes()
            => from t in typeof(WordTutorApplication).Assembly.GetTypes()
               where t.IsImmutableType()
               select t;
    }
}
