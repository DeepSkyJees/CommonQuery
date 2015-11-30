using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonQuery.Builder
{
    /// <summary>
    ///     Class LikeTransformProvider.
    /// </summary>
    internal class LikeTransformProvider : ITransFormProvider
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
            return item.Method == QueryMethod.Like;
        }

        /// <summary>
        ///     Transforms the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="type">The type.</param>
        /// <returns>IEnumerable&lt;ConditionItem&gt;.</returns>
        public IEnumerable<ConditionItem> Transform(ConditionItem item, Type type)
        {
            var str = item.Value.ToString();
            var keyWords = str.Split('*');
            if (keyWords.Length == 1)
            {
                return new[] { new ConditionItem(item.Field, QueryMethod.Contains, item.Value) };
            }
            var list = new List<ConditionItem>();
            if (!string.IsNullOrEmpty(keyWords.First()))
                list.Add(new ConditionItem(item.Field, QueryMethod.StartsWith, keyWords.First()));
            if (!string.IsNullOrEmpty(keyWords.Last()))
                list.Add(new ConditionItem(item.Field, QueryMethod.EndsWith, keyWords.Last()));
            for (int i = 1; i < keyWords.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(keyWords[i]))
                    list.Add(new ConditionItem(item.Field, QueryMethod.Contains, keyWords[i]));
            }
            return list;
        }

        #endregion ITransFormProvider Members
    }
}