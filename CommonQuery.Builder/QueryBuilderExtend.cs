﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CommonQuery.Builder
{
    /// <summary>
    ///     Class BaseQueryableExtend.
    /// </summary>
    public static class BaseQueryableExtend
    {
        /// <summary>
        ///     Orders the by.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="property">The property.</param>
        /// <param name="isAscending">if set to <c>true</c> [is ascending].</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        /// <exception cref="System.Exception">the TEntity has not  + property +  property</exception>
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, string property, bool isAscending)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity), "it");
            Expression body = param;
            if (Nullable.GetUnderlyingType(body.Type) != null)
                body = Expression.Property(body, "Value");

            PropertyInfo sortProperty = typeof(TEntity).GetProperty(property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (sortProperty == null)
                throw new Exception("the TEntity has not " + property + " property");
            body = Expression.MakeMemberAccess(body, sortProperty);
            LambdaExpression keySelectorLambda = Expression.Lambda(body, param);
            string queryMethod = isAscending ? "OrderBy" : "OrderByDescending";
            query = query.Provider.CreateQuery<TEntity>(Expression.Call(typeof(Queryable), queryMethod,
                new[] { typeof(TEntity), body.Type },
                query.Expression,
                Expression.Quote(keySelectorLambda)));
            return query;
        }

        /// <summary>
        ///     Removes the where.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="predicate">The predicate.</param>
        public static void RemoveWhere<TEntity>(this ICollection<TEntity> collection, Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var item in collection.AsQueryable().Where(predicate).ToArray())
            {
                collection.Remove(item);
            }
        }

        /// <summary>
        ///     Updates the specified update action.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="updateAction">The update action.</param>
        public static void Update<TEntity>(this IQueryable<TEntity> query, Action<TEntity> updateAction)
        {
            foreach (var item in query.ToArray())
            {
                updateAction(item);
            }
        }

        /// <summary>
        ///     Updates the specified update action.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="updateAction">The update action.</param>
        public static void Update<TEntity>(this ICollection<TEntity> collection, Action<TEntity> updateAction)
        {
            collection.AsQueryable().Update(updateAction);
        }

        #region

        /// <summary>
        ///     Wheres the specified CND.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="cnd">The CND.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, SearchCondition cnd)
        {
            var query = source;

            Specifications specification = new Specifications();

            specification.Clear();
            foreach (var item in cnd.Items)
            {
                if (!cnd.IsOrRelateion)
                {
                    specification.AndAlso(item.Field, item.Value, item.Method);
                }
                else
                {
                    specification.Or(item.Field, item.Value, item.Method, "Group1");
                }
            }

            if (specification.ConItemsCount > 0)
                query = query.Where(specification.GetExpression<TEntity>());

            return query;
        }

        #endregion

        #region

        /// <summary>
        ///     Wheres the specified qb.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="qb">The qb.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        /// <exception cref="System.Exception">Please Set Default Sort Field</exception>
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, BaseQueryBuilder qb)
        {
            var query = source.Where((SearchCondition)qb);

            if (qb.DefaultSort && typeof(TEntity).GetProperty("SortIndex") != null)
            {
                qb.SortField = "SortIndex";
                qb.SortOrder = "asc";
            }

            qb.TotolCount = query.Count();

            if (!string.IsNullOrEmpty(qb.SortField))
            {
                query = query.OrderBy(qb.SortField, string.Equals(qb.SortOrder, SortMode.Asc.ToString(), StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                throw new Exception("Please Set Default Sort Field");
                //query = query.OrderBy<TEntity>("ID", false);
            }

            if (qb.PageSize == 0)
                return query;

            query = query.Skip(qb.PageSize * qb.PageIndex).Take(qb.PageSize);

            return query;
        }

        #endregion

        #region

        #endregion
    }
}