﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace CommonQuery.Builder
{
    /// <summary>
    /// Class LambdaExpressionGenerator.
    /// </summary>
    internal class LambdaExpressionGenerator
    {
        #region

        /// <summary>
        ///     The expression dictionary
        /// </summary>
        private static readonly Dictionary<QueryMethod, Func<Expression, Expression, Expression>> ExpressionDict =
            new Dictionary<QueryMethod, Func<Expression, Expression, Expression>>
            {
                {
                    QueryMethod.Equal,
                    (left, right) =>
                    {
                        var canParse =  double.TryParse(right.ToString(), out double result);
                        if (right.Type == typeof(string) && !canParse)
                        {

                            var methodInfo = typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) });
                            return Expression.Equal(Expression.Call(left, methodInfo, right),Expression.Constant(0, typeof(Int32)));
                        }
                        return Expression.Equal(left, right);
                    }
                },
                {
                    QueryMethod.GreaterThan,
                    (left, right) =>
                    {
                        var canParse =  double.TryParse(right.ToString(), out double result);
                        if (right.Type == typeof(string) && !canParse)
                        {

                            var methodInfo = typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) });
                            return Expression.GreaterThan(Expression.Call(left, methodInfo, right),Expression.Constant(0, typeof(Int32)));
                        }
                        return Expression.GreaterThan(left, right);
                    }
                },
                {
                    QueryMethod.GreaterThanOrEqual,
                    (left, right) =>
                    {
                        var canParse =  double.TryParse(right.ToString(), out double result);
                        if (right.Type == typeof(string) && !canParse)
                        {

                            var methodInfo = typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) });
                            return Expression.GreaterThanOrEqual(Expression.Call(left, methodInfo, right),Expression.Constant(0, typeof(Int32)));
                        }
                        return Expression.GreaterThanOrEqual(left, right);
                    }
                },
                {
                    QueryMethod.LessThan,
                    (left, right) =>
                    {
                        if (right.Type == typeof(string))
                        {

                            var methodInfo = typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) });
                            return Expression.LessThan(Expression.Call(left, methodInfo, right),Expression.Constant(0, typeof(Int32)));
                        }
                        
                        return Expression.LessThan(left, right);
                    }
                },
                {
                    QueryMethod.LessThanOrEqual,
                    (left, right) =>
                    {
                        var canParse =  double.TryParse(right.ToString(), out double result);
                        if (right.Type == typeof(string) && !canParse)
                        {

                            var methodInfo = typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) });
                            return Expression.LessThanOrEqual(Expression.Call(left, methodInfo, right),Expression.Constant(0, typeof(Int32)));
                        }
                        return Expression.LessThanOrEqual(left, right);
                    }
                },
                {
                    QueryMethod.Contains,
                    (left, right) =>
                    {

                        var ignoreCase = Expression.Constant(StringComparison.CurrentCultureIgnoreCase);
                        if (left.Type != typeof(string)) return null;
                        var resultExp = Expression.Call(left, typeof(string).GetMethod("Contains",new []{typeof(string),typeof(StringComparison)}), right,ignoreCase);

                        return resultExp;
                    }
                },
                {
                    QueryMethod.StdIn,
                    (left, right) =>
                    {
                        if (!right.Type.IsArray) return null;
                        MethodCallExpression resultExp =
                            Expression.Call(
                                typeof(Enumerable),
                                "Contains",
                                new[] { left.Type },
                                right,
                                left);

                        return resultExp;
                    }
                },
                {
                    QueryMethod.NotEqual,
                    (left, right) => Expression.NotEqual(left, right)
                },
                {
                    QueryMethod.StartsWith,
                    (left, right) => left.Type != typeof(string) ? null : 
                        Expression.Call(left, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), right)
                },
                {
                    QueryMethod.EndsWith,
                    (left, right) => left.Type != typeof(string) ? null : 
                        Expression.Call(left, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), right)
                },
                {
                    QueryMethod.DateTimeLessThanOrEqual,
                    (left, right) =>  Expression.LessThanOrEqual(left, right)
                },
                {
                    QueryMethod.NotLike,
                    (left, right) =>
                    {
                        if (right.Type.IsArray) return null;
                        var ignoreCase = Expression.Constant(StringComparison.CurrentCultureIgnoreCase);
                        UnaryExpression resultExp =
                            Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains",new[] {typeof(string),typeof(StringComparison)}), right,ignoreCase));

                        return resultExp;
                    }
                },
            };

        #endregion

        /// <summary>
        ///     The transform providers
        /// </summary>
        private static readonly List<ITransFormProvider> TransformProviders;

        /// <summary>
        ///     Initializes static members of the <see cref="LambdaExpressionGenerator" /> class.
        /// </summary>
        static LambdaExpressionGenerator()
        {
            TransformProviders = new List<ITransFormProvider>
            {
                new InTransformProvider(),
                new LikeTransformProvider(),
                new UnixTimeTransformProvider(),
                new DateBlockTransFormProvider(),
                new BetweenTransformProvider(),
                //new ContainsTransformProvider()
            };
        }

        /// <summary>
        ///     Generates the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        public static Expression<Func<T, bool>> GenerateExpression<T>(IEnumerable<ConditionItem> items)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "c");
            var body = GetExpressoinBody<T>(param, items);
            var expression = Expression.Lambda<Func<T, bool>>(body, param);
            return expression;
        }

        /// <summary>
        ///     Generates the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        public static Expression<Func<T, bool>> GenerateExpression<T>(params ConditionItem[] items)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "c");
            //Create Body
            var body = GetExpressoinBody<T>(param, items);
            //Join Body
            var expression = Expression.Lambda<Func<T, bool>>(body, param);
            return expression;
        }

        #region

        /// <summary>
        ///     Changes the type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="conversionType">Type of the conversion.</param>
        /// <returns>System.Object.</returns>
        private static object ChangeType(object value, Type conversionType)
        {
            return value == null ? null : Convert.ChangeType(value, TypeUtil.GetUnNullableType(conversionType));
        }

        /// <summary>
        ///     Changes the type to expression.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="conversionType">Type of the conversion.</param>
        /// <returns>Expression.</returns>
        private static Expression ChangeTypeToExpression(ConditionItem item, Type conversionType)
        {
            if (item.Value == null) return Expression.Constant(item.Value, conversionType);

            #region 数组

            if (item.Method == QueryMethod.StdIn)
            {
                var arr = (item.Value as Array);
                var expList = new List<Expression>();

                if (arr != null)
                    expList.AddRange(arr.Cast<object>().Select((t, i) => ChangeType(arr.GetValue(i), conversionType)).Select(newValue => Expression.Constant(newValue, conversionType)));
                return Expression.NewArrayInit(conversionType, expList);
            }

            #endregion

            var objValue = item.Value;
            var elementType = TypeUtil.GetUnNullableType(conversionType);
            //var value = item.Value;
            var value = Convert.ChangeType(objValue is JsonElement ? objValue.ToString() : objValue, elementType);
            return Expression.Constant(value, conversionType);
        }

        /// <summary>
        ///     Gets the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The parameter.</param>
        /// <param name="item">The item.</param>
        /// <returns>Expression.</returns>
        private static Expression GetExpression<T>(ParameterExpression param, ConditionItem item)
        {
            LambdaExpression exp = GetPropertyLambdaExpression<T>(item, param);
            foreach (var provider in TransformProviders.Where(provider => provider.Match(item, exp.Body.Type)))
            {
                if (item.Method == QueryMethod.Contains)
                {

                    return GetGroupExpression<T>(param, provider.Transform(item, exp.Body.Type), Expression.Or);
                }
                else
                {

                    return GetGroupExpression<T>(param, provider.Transform(item, exp.Body.Type), Expression.AndAlso);
                }
            }
            var constant = ChangeTypeToExpression(item, exp.Body.Type);
            return ExpressionDict[item.Method](exp.Body, constant);
        }

        /// <summary>
        ///     Gets the expressoin body.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The parameter.</param>
        /// <param name="items">The items.</param>
        /// <returns>Expression.</returns>
        private static Expression GetExpressoinBody<T>(ParameterExpression param, IEnumerable<ConditionItem> items)
        {
            var list = new List<Expression>();
            var enumerable = items as ConditionItem[] ?? items.ToArray();
            var andList = enumerable.Where(c => string.IsNullOrEmpty(c.OrGroup));
            var conditionItems = andList as ConditionItem[] ?? andList.ToArray();
            if (conditionItems.Count() != 0)
            {
                list.Add(GetGroupExpression<T>(param, conditionItems, Expression.AndAlso));
            }
            var orGroupByList = enumerable.Where(c => !string.IsNullOrEmpty(c.OrGroup)).GroupBy(c => c.OrGroup);
            list.AddRange(from @group in orGroupByList where @group.Count() != 0 select GetGroupExpression<T>(param, @group, Expression.OrElse));
            return list.Aggregate(Expression.AndAlso);
        }

        /// <summary>
        ///     Gets the group expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The parameter.</param>
        /// <param name="items">The items.</param>
        /// <param name="func">The function.</param>
        /// <returns>Expression.</returns>
        private static Expression GetGroupExpression<T>(ParameterExpression param, IEnumerable<ConditionItem> items, Func<Expression, Expression, Expression> func)
        {
            var list = items.Select(item => GetExpression<T>(param, item));
            return list.Aggregate(func);
        }

        /// <summary>
        ///     Gets the property lambda expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>LambdaExpression.</returns>
        private static LambdaExpression GetPropertyLambdaExpression<T>(ConditionItem item, ParameterExpression param)
        {
            var props = item.Field.Split('.');
            Expression propertyAccess = param;
            var typeOfProp = typeof(T);
            int i = 0;
            do
            {
                PropertyInfo property = typeOfProp.GetProperty(props[i], BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                if (property == null) return null;
                typeOfProp = property.PropertyType;
                propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                i++;
            } while (i < props.Length);

            return Expression.Lambda(propertyAccess, param);
        }

        #endregion
    }
}