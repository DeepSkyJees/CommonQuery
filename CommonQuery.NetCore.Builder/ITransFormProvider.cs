using System;
using System.Collections.Generic;

namespace CommonQuery.NetCore.Builder
{
    /// <summary>
    ///     Interface ITransFormProvider
    /// </summary>
    internal interface ITransFormProvider
    {
        /// <summary>
        ///     Matches the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Match(ConditionItem item, Type type);

        /// <summary>
        ///     Transforms the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="type">The type.</param>
        /// <returns>IEnumerable&lt;ConditionItem&gt;.</returns>
        IEnumerable<ConditionItem> Transform(ConditionItem item, Type type);
    }
}