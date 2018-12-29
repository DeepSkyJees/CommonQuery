using System;
using System.Collections.Generic;

namespace CommonQuery.NetCore.Builder
{
    /// <summary>
    ///     Class InTransformProvider.
    /// </summary>
    internal class InTransformProvider : ITransFormProvider
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
            return item.Method == QueryMethod.In;
        }

        /// <summary>
        ///     Transforms the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="type">The type.</param>
        /// <returns>IEnumerable&lt;ConditionItem&gt;.</returns>
        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            var arr = (item.Value as Array);
            if (arr == null)
            {
                var arrStr = item.Value.ToString();
                if (!string.IsNullOrEmpty(arrStr))
                {
                    arr = arrStr.Split(',');
                }
            }
            return new[] { new ConditionItem(item.Field, QueryMethod.StdIn, arr) };
        }

        #endregion ITransFormProvider Members
    }
}