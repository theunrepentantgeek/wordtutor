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

        [Theory]
        [MemberData(nameof(FindWithersOfImmutableTypes))]
        public void WithersShouldReturnTheirDeclaringType(MethodInfo wither)
        {
            wither.ReturnType.Should().Be(
                wither.DeclaringType,
                $"wither {wither.Name} of type {wither.DeclaringType.Name} "
                + $"should return {wither.DeclaringType.Name}");
        }

        [Theory]
        [MemberData(nameof(FindWithersOfImmutableTypes))]
        public void WitherParametersShouldIdentifyProperties(MethodInfo wither)
        {
            var properties = (
                from p in wither.DeclaringType.GetProperties()
                select p.Name
            ).ToHashSet(StringComparer.OrdinalIgnoreCase);
            wither.GetParameters().Should().OnlyContain(
                p => properties.Contains(p.Name),
                $"parameters should be one of {string.Join(", ", properties)}");
        }

        public static IEnumerable<object[]> FindWithersOfImmutableTypes()
            => from t in GetForImmutableTypes()
               from m in t.GetMethods()
               where m.Name.StartsWith("With", StringComparison.Ordinal)
               select new object[] { m };

        [Theory]
        [MemberData(nameof(FindClearersOfImmutableTypes))]
        public void ClearersShouldReturnTheirDeclaringType(MethodInfo clearer)
        {
            clearer.ReturnType.Should().Be(
                clearer.DeclaringType,
                $"wither {clearer.Name} of type {clearer.DeclaringType.Name} "
                + $"should return {clearer.DeclaringType.Name}");
        }

        [Theory]
        [MemberData(nameof(FindClearersOfImmutableTypes))]
        public void ClearersShouldHaveNamesIdentifyingTheClearedProperty(MethodInfo clearer)
        {
            var propertyName = clearer.Name.Substring("Clear".Length);
            clearer.DeclaringType.GetProperties()
                .Should().Contain(
                    p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase),
                    $"no property {propertyName} was found on {clearer.DeclaringType.Name}");
        }

        public static IEnumerable<object[]> FindClearersOfImmutableTypes()
            => from t in GetForImmutableTypes()
               from m in t.GetMethods()
               where m.Name.StartsWith("Clear", StringComparison.Ordinal)
               select new object[] { m };

        private static IEnumerable<Type> GetForImmutableTypes()
            => from t in typeof(WordTutorApplication).Assembly.GetTypes()
               where t.IsImmutableType()
               select t;
    }
}
