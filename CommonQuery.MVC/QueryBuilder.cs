using CommonQuery.Builder;
using System.Web.Mvc;

namespace CommonQuery.MVC
{
    [ModelBinder(typeof(QueryBuilderBinder))]
    public class QueryBuilder : BaseQueryBuilder
    {
    }
}