using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace WordTutor.Core.Tests
{
    public static class TypeExtensions
    {
        private static HashSet<Type> _systemImmutableTypes
            = new HashSet<Type>
            {
                typeof(int),
                typeof(string),
                typeof(bool),
                typeof(IImmutableSet<>)
            };

        public static bool IsImmutableType(this Type type)
        {
            if (type is null)
            {
                return false;
            }

            if (type.GetCustomAttribute<ImmutableAttribute>(inherit: false) is object)
            {
                return true;
            }

            if (_systemImmutableTypes.Contains(type))
            {
                return true;
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var definition = type.GetGenericTypeDefinition();
                var parameters = type.GetGenericArguments();
                return definition.IsImmutableType()
                    && parameters.All(t => t.IsImmutableType());
            }

            return false;
        }
    }
}
