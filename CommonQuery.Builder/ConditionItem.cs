using System.ComponentModel;

namespace CommonQuery.Builder
{
    /// <summary>
    ///     Enum QueryMethod
    /// </summary>
    public enum QueryMethod
    {
        /// <summary>
        ///     The equal
        /// </summary>
        Equal = 0,

        /// <summary>
        ///     The less than
        /// </summary>
        LessThan = 1,

        /// <summary>
        ///     The greater than
        /// </summary>
        GreaterThan = 2,

        /// <summary>
        ///     The less than or equal
        /// </summary>
        LessThanOrEqual = 3,

        /// <summary>
        ///     The greater than or equal
        /// </summary>
        GreaterThanOrEqual = 4,

        /// <summary>
        ///     The like
        /// </summary>
        Like = 6,

        /// <summary>
        ///     In
        /// </summary>
        In = 7,

        /// <summary>
        ///     The date block
        /// </summary>
        DateBlock = 8,

        /// <summary>
        ///     The not equal
        /// </summary>
        NotEqual = 9,

        /// <summary>
        ///     The starts with
        /// </summary>
        StartsWith = 10,

        /// <summary>
        ///     The ends with
        /// </summary>
        EndsWith = 11,

        /// <summary>
        ///     The contains
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Contains = 12,

        /// <summary>
        ///     The standard in
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        StdIn = 13,

        /// <summary>
        ///     The date time less than or equal
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        DateTimeLessThanOrEqual = 14,

        /// <summary>
        ///     The between
        /// </summary>
        Between= 15,


        /// <summary>
        /// The not like
        /// </summary>
        NotLike = 16,
    }

    /// <summary>
    ///     Enum SortMode
    /// </summary>
    public enum SortMode
    {
        /// <summary>
        ///     The asc
        /// </summary>
        Asc,

        /// <summary>
        ///     The desc
        /// </summary>
        Desc
    }

    /// <summary>
    ///     Class ConditionItem.
    /// </summary>
    public class ConditionItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConditionItem" /> class.
        /// </summary>
        public ConditionItem()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConditionItem" /> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="method">The method.</param>
        /// <param name="val">The value.</param>
        public ConditionItem(string field, QueryMethod method, object val)
        {
            this.Field = field;
            this.Method = method;
            this.Value = val;
        }

        /// <summary>
        ///     Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        public string Field { get; set; }

        /// <summary>
        ///     Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>
        public QueryMethod Method { get; set; }

        /// <summary>
        ///     Gets or sets the or group.
        /// </summary>
        /// <value>The or group.</value>
        public string OrGroup { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; set; }
    }
}