using System;
using System.Collections.Generic;

namespace CommonQuery.NetCore.Builder
{
    /// <summary>
    ///     Class UnixTimeTransformProvider.
    /// </summary>
    internal class UnixTimeTransformProvider : ITransFormProvider
    {
        #region ITransFormProvider Members

        /// <summary>
        ///     Matches the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Match(ConditionItem item, Type type)
        {
            var elementType = TypeUtil.GetUnNullableType(type);
            return ((elementType == typeof(int) && !(item.Value is int))
                    || (elementType == typeof(long) && !(item.Value is long))
                    || (elementType == typeof(DateTime) && !(item.Value is DateTime))
                )
                   && item.Value.ToString().Contains("-");
        }

        /// <summary>
        ///     Transforms the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="type">The type.</param>
        /// <returns>IEnumerable&lt;ConditionItem&gt;.</returns>
        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            DateTime willTime;
            Type instanceType = TypeUtil.GetUnNullableType(type);
            if (DateTime.TryParse(item.Value.ToString(), out willTime))
            {
                var method = item.Method;

                if (method == QueryMethod.LessThan || method == QueryMethod.LessThanOrEqual)
                {
                    method = QueryMethod.DateTimeLessThanOrEqual;
                    if (willTime.Hour == 0 && willTime.Minute == 0 && willTime.Second == 0)
                    {
                        willTime = willTime.AddDays(1).AddMilliseconds(-1);
                    }
                }
                object value = null;
                if (instanceType == typeof(DateTime))
                {
                    value = willTime;
                }
                else if (instanceType == typeof(int))
                {
                    value = (int)UnixTime.FromDateTime(willTime);
                }
                else if (instanceType == typeof(long))
                {
                    value = UnixTime.FromDateTime(willTime);
                }
                return new[] { new ConditionItem(item.Field, method, value) };
            }

            return new[] { new ConditionItem(item.Field, item.Method, Convert.ChangeType(item.Value, type)) };
        }

        #endregion ITransFormProvider Members
    }
}