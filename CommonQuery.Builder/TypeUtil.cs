using System;
using System.ComponentModel;

namespace CommonQuery.Builder
{
    /// <summary>
    ///     Class TypeUtil.
    /// </summary>
    internal class TypeUtil
    {
        /// <summary>
        ///     Gets the type of the un nullable.
        /// </summary>
        /// <param name="conversionType">Type of the conversion.</param>
        /// <returns>Type.</returns>
        public static Type GetUnNullableType(Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return conversionType;
        }
    }
}