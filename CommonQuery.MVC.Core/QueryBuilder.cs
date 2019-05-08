using CommonQuery.Builder;
using Microsoft.AspNetCore.Mvc;

namespace CommonQuery.MVC.Core
{
    [ModelBinder(typeof(QueryBuilderBinder))]
    public class QueryBuilder:BaseQueryBuilder
    {
        
    }
}