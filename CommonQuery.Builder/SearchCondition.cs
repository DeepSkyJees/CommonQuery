using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UseStringInterpolation

namespace CommonQuery.Builder
{
    /// <summary>
    ///     Class SearchCondition.
    /// </summary>
    public class SearchCondition
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchCondition" /> class.
        /// </summary>
        protected SearchCondition()
        {
            this.Items = new List<ConditionItem>();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is or relateion.
        /// </summary>
        /// <value><c>true</c> if this instance is or relateion; otherwise, <c>false</c>.</value>
        public bool IsOrRelateion { get; set; }

        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<ConditionItem> Items { get; set; }

        /// <summary>
        ///     Adds the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="method">The method.</param>
        /// <param name="val">The value.</param>
        /// <returns>SearchCondition.</returns>
        public SearchCondition Add(string field, QueryMethod method, object val)
        {
            ConditionItem item = new ConditionItem(field, method, val);
            this.Items.Add(item);
            return this;
        }

        /// <summary>
        ///     Adds the between condition.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="beginVal">The begin value.</param>
        /// <param name="endVal">The end value.</param>
        /// <returns>SearchCondition.</returns>
        public SearchCondition AddBetweenCondition(string field, object beginVal, object endVal)
        {
            ConditionItem item = new ConditionItem
            {
                Field = field,
                Method = QueryMethod.Between,
                Value = new[] { beginVal, endVal }
            };
            this.Items.Add(item);

            return this;
        }

        /// <summary>
        ///     Clears the condition items.
        /// </summary>
        public void ClearConditionItems()
        {
            this.Items.Clear();
        }

        /// <summary>
        ///     Gets the where string.
        /// </summary>
        /// <param name="hasWhere">if set to <c>true</c> [has where].</param>
        /// <returns>System.String.</returns>
        public string GetWhereString(bool hasWhere = true)
        {
            string strWhere = "";
            for (int i = 0; i < this.Items.Count(); i++)
            {
                var item = this.Items[i];
                string str = this.GetWhereString(item);

                if (i == 0)
                {
                    if (hasWhere)
                        strWhere += " where " + str;
                    else
                        strWhere += str;
                }
                else
                {
                    if (this.IsOrRelateion)
                    {
                        strWhere += " or " + str;
                    }
                    else
                    {
                        strWhere += " and " + str;
                    }
                }
            }

            return strWhere;
        }

        #region 私有方法

        /// <summary>
        ///     Gets the where string.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception">Query conditions cannot be converted to date</exception>
        private string GetWhereString(ConditionItem item)
        {
            string str = "";

            switch (item.Method)
            {
                case QueryMethod.Contains:
                case QueryMethod.StdIn:
                case QueryMethod.DateTimeLessThanOrEqual:
                    break;

                case QueryMethod.Equal:
                    // ReSharper disable once UseStringInterpolation
                    str = string.Format("{0} = '{1}'", item.Field, item.Value);
                    //str = $"{item.Field}='{item.Value}'";
                    break;

                case QueryMethod.LessThan:
                    // ReSharper disable once UseStringInterpolation
                    str = string.Format("{0} < '{1}'", item.Field, item.Value);
                    //str = $"{item.Field}<'{item.Value}'";
                    break;

                case QueryMethod.GreaterThan:
                    // ReSharper disable once UseStringInterpolation
                    str = string.Format("{0} > '{1}'", item.Field, item.Value);
                    //str = $"{item.Field}>'{item.Value}'";
                    break;

                case QueryMethod.LessThanOrEqual:
                    // ReSharper disable once UseStringInterpolation
                    str = string.Format("{0} <= '{1}'", item.Field, item.Value);
                    //str = $"{item.Field}<='{item.Value}'";
                    break;

                case QueryMethod.GreaterThanOrEqual:
                    // ReSharper disable once UseStringInterpolation
                    str = string.Format("{0} >= '{1}'", item.Field, item.Value);
                    //str = $"{item.Field}>='{item.Value}'";
                    break;

                case QueryMethod.Like:
                    // ReSharper disable once UseStringInterpolation
                    str = string.Format("{0} like '%{1}%'", item.Field, item.Value);
                    //str = $"{item.Field} like '%{item.Value}%'";
                    break;

                case QueryMethod.In:
                    string strInValue;
                    if (item.Value is ICollection)
                    {
                        ICollection<string> collection = item.Value as ICollection<string>;
                        strInValue = string.Join("','", collection.ToArray());
                    }
                    else
                    {
                        strInValue = item.Value.ToString().Replace(",", "','");
                    }
                    // ReSharper disable once UseStringInterpolation
                    str = string.Format("{0} in '{1}'", item.Field, strInValue);
                    //str = $"{item.Field} in ('{strInValue}')";
                    break;

                case QueryMethod.DateBlock:
                    DateTime dt;
                    if (!DateTime.TryParse(item.Value.ToString(), out dt))
                    {
                        throw new Exception("Query conditions cannot be converted to date");
                    }
                    string start = dt.Date.ToString("yyyy-MM-dd");
                    string end = dt.Date.AddDays(1).ToString("yyyy-MM-dd");
                    // ReSharper disable once UseStringInterpolation
                    str = string.Format("{0} between '{1}' and '{2}'", item.Field, start, end);
                    break;

                case QueryMethod.NotEqual:
                    str = string.Format("{0} <> '{1}'", item.Field, item.Value);
                    //str = $"{item.Field}<>'{item.Value}'";
                    break;

                case QueryMethod.StartsWith:
                    str = string.Format("{0} like '{1}%'", item.Field, item.Value);
                    break;

                case QueryMethod.EndsWith:
                    // ReSharper disable once UseStringInterpolation
                    str = string.Format("{0} like '%{1}'", item.Field, item.Value);
                    break;

                case QueryMethod.Between:
                    object[] objs = item.Value as object[];
                    if (objs != null)
                        // ReSharper disable once UseStringInterpolation
                        str = string.Format("{0} between '{1}' and '{2}'", item.Field, objs[0], objs[1]);
                    break;
            }

            return str;
        }

        #endregion 私有方法
    }
}