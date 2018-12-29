using System;
using System.Collections.Generic;

namespace CommonQuery.NetCore.Builder
{
    /// <summary>
    ///     Class BetweenTransformProvider.
    /// </summary>
    internal class BetweenTransformProvider : ITransFormProvider
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
            return item.Method == QueryMethod.Between;
        }

        /// <summary>
        ///     Transforms the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="type">The type.</param>
        /// <returns>IEnumerable&lt;ConditionItem&gt;.</returns>
        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            object[] objs = item.Value as object[];
            if (objs == null)
            {
                return new[]
                {
                    new ConditionItem(item.Field, QueryMethod.GreaterThanOrEqual, ""),
                    new ConditionItem(item.Field, QueryMethod.LessThan, "")
                };
            }
            if (objs.Length < 2)
            {
                return new[]
                {
                    new ConditionItem(item.Field, QueryMethod.GreaterThanOrEqual, ""),
                    new ConditionItem(item.Field, QueryMethod.LessThan, "")
                };
            }
            return new[]
            {
                new ConditionItem(item.Field, QueryMethod.GreaterThanOrEqual, objs[0]),
                new ConditionItem(item.Field, QueryMethod.LessThan, objs[1])
            };
        }

        #endregion ITransFormProvider Members
    }
}