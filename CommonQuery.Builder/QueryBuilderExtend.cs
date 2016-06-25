using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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
            if (qb.Items.Count() != 0)
            {
                var fields = qb.Items.Select(p => p.Field).ToList();
                var entityFields = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).Select(p => p.Name);
                var noneFields = fields.FindAll(p => !entityFields.Contains(p));
                if (noneFields.Count() != 0)
                {
                    throw new Exception($"the object dose not have {string.Join(",", noneFields)}fields");
                }
            }
            var query = source.Where((SearchCondition)qb);

            //if (qb.DefaultSort && typeof(TEntity).GetProperty(qb.SortField, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance) == null)
            //{
            //    qb.SortField = "SortIndex";
            //    qb.SortOrder = "asc";
            //}

            qb.TotolCount = query.Count();

            if (!string.IsNullOrEmpty(qb.SortField)&& qb.DefaultSort)
            {
                query = query.OrderBy(qb.SortField, string.Equals(qb.SortOrder, SortMode.Asc.ToString(), StringComparison.CurrentCultureIgnoreCase));
            }
            else if(qb.DefaultSort)
            {
                throw new Exception("Please Set Default Sort Field");
                //query = query.OrderBy<TEntity>("ID", false);
            }

            if (qb.PageSize == 0)
                return query;
            if (qb.NeedPaging)
            {
                query = query.Where(qb.PageSize,qb.PageIndex);
            }
            

            return query;
        }

        /// <summary>
        /// Wheres the specified qb.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        /// <exception cref="System.Exception">Please Set Default Sort Field</exception>
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, int pageSize,int pageIndex)
        {
            if (source == null)
            {
                throw  new AggregateException("source can not be null");
            }
            source = source.Skip(pageSize * pageIndex).Take(pageSize);

            return source;
        }

        #endregion
        
    }
}