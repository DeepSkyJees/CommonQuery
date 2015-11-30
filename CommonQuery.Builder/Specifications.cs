using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CommonQuery.Builder
{
    /// <summary>
    ///     Class Specifications.
    /// </summary>
    public class Specifications : ISpecifications
    {
        /// <summary>
        ///     The _con items
        /// </summary>
        private readonly List<ConditionItem> conItems;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Specifications" /> class.
        /// </summary>
        public Specifications()
        {
            this.conItems = new List<ConditionItem>();
        }

        #region ISpecifications Members

        /// <summary>
        ///     Gets the con items count.
        /// </summary>
        /// <value>The con items count.</value>
        /// <remarks>只读属性</remarks>
        public int ConItemsCount
        {
            get { return this.conItems.Count; }
        }

        /// <summary>
        ///     Gets or sets the dir.
        /// </summary>
        /// <value>The dir.</value>
        public SortMode Dir { get; set; } = SortMode.Desc;

        /// <summary>
        /// </summary>
        public string SortField { get; set; } = string.Empty;

        /// <summary>
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="val"></param>
        /// <param name="method"></param>
        public void AndAlso(string prop, object val, QueryMethod method)
        {
            this.conItems.Add(new ConditionItem(prop, method, val));
        }

        /// <summary>
        /// </summary>
        public void Clear()
        {
            this.conItems.Clear();
            this.SortField = string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="val"></param>
        public void DefaultAndAlso(string prop, object val)
        {
            this.conItems.Add(new ConditionItem(prop, QueryMethod.Like, val));
        }

        /// <summary>
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="val"></param>
        public void DefaultOr(string prop, object val)
        {
            ConditionItem conItem = new ConditionItem(prop, QueryMethod.Like, val) { OrGroup = "OrGroup_1" };
            this.conItems.Add(conItem);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> GetExpression<TEntity>()
        {
            return LambdaExpressionGenrator.GenerateExpression<TEntity>(this.conItems);
        }

        /// <summary>
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="val"></param>
        /// <param name="method"></param>
        /// <param name="group"></param>
        public void Or(string prop, object val, QueryMethod method, string @group)
        {
            ConditionItem conItem = new ConditionItem(prop, method, val) { OrGroup = @group };
            this.conItems.Add(conItem);
        }

        #endregion ISpecifications Members
    }
}