// Decompiled with JetBrains decompiler
// Type: CommonQuery.MVC.Core.QueryBuilderBinder
// Assembly: CommonQuery.MVC.Core, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B631EEBD-8325-4CFA-92DA-F9541AB9EEB4
// Assembly location: C:\Users\yuliang\.nuget\packages\commonquery.mvc.core\5.0.0\lib\net5.0\CommonQuery.MVC.Core.dll

using CommonQuery.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonQuery.MVC.Core
{
    public class QueryBuilderBinder : IModelBinder
    {
        public static void AddSearchItem(QueryBuilder model, string key, string val)
        {
            if (model == null)
                model = new QueryBuilder();
            string str1 = "";
            string[] strArray = key.Split("$', ')', '}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length != 2)
                return;
            string str2 = QueryBuilderBinder.SearchMethodAdapter(strArray[0]);
            string str3 = strArray[1];
            if (string.IsNullOrEmpty(str2) || string.IsNullOrEmpty(str3))
                return;
            ConditionItem conditionItem = new ConditionItem()
            {
                Field = str3,
                Value = (object)val.Trim(),
                OrGroup = str1,
                Method = (QueryMethod)Enum.Parse(typeof(QueryMethod), str2)
            };
            model.Items.Add(conditionItem);
        }

        private static string SearchMethodAdapter(string method)
        {
            string str = "";
            switch (method.ToUpper())
            {
                case "EQ":
                    str = "Equal";
                    break;
                case "EW":
                    str = "EndsWith";
                    break;
                case "FR":
                    str = "GreaterThanOrEqual";
                    break;
                case "GT":
                    str = "GreaterThan";
                    break;
                case "IGNORE":
                    str = "";
                    break;
                case "IN":
                    str = "In";
                    break;
                case "LK":
                    str = "Like";
                    break;
                case "LT":
                    str = "LessThan";
                    break;
                case "NL":
                    str = "Not Like";
                    break;
                case "SW":
                    str = "StartsWith";
                    break;
                case "TO":
                    str = "LessThanOrEqual";
                    break;
                case "UE":
                    str = "NotEqual";
                    break;
            }
            return str;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            QueryBuilder model = (QueryBuilder)(bindingContext.Model ?? (object)new QueryBuilder());
            IFormCollection form = bindingContext.HttpContext.Request.Form;
            string str1 = (string)form["prefix"];
            if (str1 == null)
                throw new Exception("it dose not have prefix,please go to set<input name='prefix' value='Jeeees' type='hidden'>");
            foreach (string key in (IEnumerable<string>)form.Keys)
            {
                if (key.StartsWith(str1 + "_", StringComparison.Ordinal))
                {
                    StringValues val = form[key];
                    if (!string.IsNullOrEmpty((string)val))
                        QueryBuilderBinder.AddSearchItem(model, key.Split('_')[1], (string)val);
                }
            }
            int result1 = 0;
            int result2 = 20;
            string str2 = string.IsNullOrEmpty((string)form["sort"]) ? "ID" : form["sort"].ToString();
            string str3 = string.IsNullOrEmpty((string)form["order"]) ? "desc" : form["order"].ToString();
            bool result3;
            bool.TryParse((string)form["isOrRelation"], out result3);
            if (!string.IsNullOrEmpty((string)form["page"]) && int.TryParse((string)form["page"], out result1))
                --result1;
            if (!string.IsNullOrEmpty((string)form["rows"]))
                int.TryParse((string)form["rows"], out result2);
            model.IsOrRelateion = result3;
            model.PageIndex = result1;
            model.PageSize = result2;
            model.SortField = str2;
            model.SortOrder = str3;
            model.DefaultSort = string.IsNullOrEmpty((string)form["sort"]);
            bindingContext.Result = ModelBindingResult.Success((object)model);
            return Task.CompletedTask;
        }
    }
}
