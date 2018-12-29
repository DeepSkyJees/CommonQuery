using System;
using System.Collections;
using System.Linq.Expressions;

namespace CommonQuery.NetCore.Builder
{
    /// <summary>
    ///     Interface ISpecifications
    /// </summary>
    public interface ISpecifications : IEnumerable
    {
        /// <summary>
        ///     Gets the con items count.
        /// </summary>
        /// <value>The con items count.</value>
        int ConItemsCount { get; }

        /// <summary>
        ///     Gets or sets the dir.
        /// </summary>
        /// <value>The dir.</value>
        SortMode Dir { get; set; }

        /// <summary>
        ///     Gets or sets the sort field.
        /// </summary>
        /// <value>The sort field.</value>
        string SortField { get; set; }

        /// <summary>
        ///     Ands the also.
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <param name="val">The value.</param>
        /// <param name="method">The method.</param>
        void AndAlso(string prop, object val, QueryMethod method);

        /// <summary>
        ///     Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Defaults the and also.
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <param name="val">The value.</param>
        void DefaultAndAlso(string prop, object val);

        /// <summary>
        ///     Defaults the or.
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <param name="val">The value.</param>
        void DefaultOr(string prop, object val);

        /// <summary>
        ///     Gets the expression.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>Expression&lt;Func&lt;TEntity, System.Boolean&gt;&gt;.</returns>
        Expression<Func<TEntity, bool>> GetExpression<TEntity>();

        /// <summary>
        ///     Ors the specified property.
        /// </summary>
        /// <param name="prop">The property.</param>
        /// <param name="val">The value.</param>
        /// <param name="method">The method.</param>
        /// <param name="group">The group.</param>
        void Or(string prop, object val, QueryMethod method, string @group);
    }
}