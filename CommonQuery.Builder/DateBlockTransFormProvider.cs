using System;
using System.Collections.Generic;

namespace CommonQuery.Builder
{
    /// <summary>
    ///     Class DateBlockTransFormProvider.
    /// </summary>
    internal class DateBlockTransFormProvider : ITransFormProvider
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
            return item.Method == QueryMethod.DateBlock;
        }

        /// <summary>
        ///     Transforms the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="type">The type.</param>
        /// <returns>IEnumerable&lt;ConditionItem&gt;.</returns>
        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            return new[]
            {
                new ConditionItem(item.Field, QueryMethod.GreaterThanOrEqual, item.Value),
                new ConditionItem(item.Field, QueryMethod.LessThan, item.Value)
            };
        }

        #endregion ITransFormProvider Members
    }
}