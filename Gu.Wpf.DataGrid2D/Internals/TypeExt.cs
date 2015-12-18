namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class TypeExt
    {
        internal static bool IsEnumerableOfT(this Type type)
        {
            var iEnumerableOfT = type.GetIEnumerableOfT();
            return iEnumerableOfT != null;
        }

        internal static Type GetEnumerableItemType(this Type type)
        {
            var enumerable = type.GetIEnumerableOfT();
            if (enumerable == null)
            {
                var message = $"Trying to get typeof(T) when type is not IEnumerable<T>, type is {type.Name}";
                throw new ArgumentException(message, nameof(type));
            }

            return enumerable.GetGenericArguments()
                             .Single();
        }

        private static Type GetIEnumerableOfT(this Type type)
        {
            var enumerable = type.GetInterfaces()
                                 .Where(i => i.IsGenericType)
                                 .SingleOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            return enumerable;
        }

        internal static bool IsEquatable(this Type type)
        {
            return type.Implements(typeof(IEquatable<>), type);
        }

        /// <summary>
        /// To check if type implements IEquatable{string}
        /// Call like this type.Implements(typeof(IEquatable{}, typeof(string))
        /// </summary>
        internal static bool Implements(this Type type, Type genericInterface, Type genericArgument)
        {
            if (type.IsInterface &&
                type.IsGenericType(genericInterface, genericArgument))
            {
                return true;
            }

            var interfaces = type.GetInterfaces();
            return interfaces.Any(i => IsGenericType(i, genericInterface, genericArgument));
        }

        internal static bool IsGenericType(this Type type, Type genericTypeDefinition, Type genericArgument)
        {
            //Ensure.IsTrue(genericTypeDefinition.IsGenericType, nameof(genericTypeDefinition), $"{nameof(genericTypeDefinition)}.{nameof(genericTypeDefinition.IsGenericType)} must be true");

            if (!type.IsGenericType)
            {
                return false;
            }

            var gtd = type.GetGenericTypeDefinition();
            if (gtd != genericTypeDefinition)
            {
                return false;
            }

            var genericArguments = type.GetGenericArguments();
            return genericArguments.Length == 1 && genericArguments[0] == genericArgument;
        }
    }
}