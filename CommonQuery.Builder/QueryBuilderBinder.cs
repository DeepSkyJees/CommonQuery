using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CommonQuery.Builder
{
    public static class QueryBuilderBinder
    {
        /// <summary>
        ///     add key=value into QueryModel.Items
        /// </summary>
        /// <param name="model">QueryModel</param>
        /// <param name="key">the name of Element (input/select/area/radio/checkbox)</param>
        /// <param name="val">the value of Element (input/select/area/radio/checkbox)</param>
        public static void AddSearchItem(QueryBuilder model, string key, string val)
        {
            if (model == null)
            {
                model = new QueryBuilder();
            }
            string orGroup = "";
            var keywords = key.Split("$', ')', '}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (keywords.Length != 2)
                return;
            var method = SearchMethodAdapter(keywords[0]);
            var field = keywords[1];
            if (string.IsNullOrEmpty(method)) return;
            if (!string.IsNullOrEmpty(field))
            {
                var item = new ConditionItem
                {
                    Field = field,
                    Value = val.Trim(),
                    OrGroup = orGroup,
                    Method = (QueryMethod)Enum.Parse(typeof(QueryMethod), method)
                };
                model.Items.Add(item);
            }
        }

        public static QueryBuilder CreateModel(string dicJsonString)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(dicJsonString);
            var model = new QueryBuilder();

            foreach (string key in dict.Keys)
            {
                var val = dict[key];
                // Handle a null value
                if (string.IsNullOrEmpty(val)) continue;
                AddSearchItem(model, key, val);
            }

            bool isOrRelation = false;
            int pageIndex = 0;
            int pageSize = 20;
            string sortField = string.IsNullOrEmpty(dict["sort"]) ? "ID" : dict["sort"];
            string sortOrder = string.IsNullOrEmpty(dict["order"]) ? "desc" : dict["order"];

            if (!string.IsNullOrEmpty(dict["isOrRelation"]))
            {
                bool.TryParse(dict["isOrRelation"], out isOrRelation);
            }

            if (!string.IsNullOrEmpty(dict["page"]))
            {
                if (int.TryParse(dict["page"], out pageIndex))
                {
                    pageIndex = pageIndex - 1;
                }
            }
            if (!string.IsNullOrEmpty(dict["rows"]))
            {
                int.TryParse(dict["rows"], out pageSize);
            }

            model.IsOrRelateion = isOrRelation;
            model.PageIndex = pageIndex;
            model.PageSize = pageSize;
            model.SortField = sortField;
            model.SortOrder = sortOrder;
            model.DefaultSort = string.IsNullOrEmpty(dict["sort"]);
            return model;
        }

        /// <summary>
        ///     Query matching mode adapter
        /// </summary>
        /// <param name="method">Simplify the matching mode</param>
        /// <returns>The full name of matched pattern</returns>
        private static string SearchMethodAdapter(string method)
        {
            string match = "";
            switch (method.ToUpper())
            {
                case "EQ":
                    match = "Equal";
                    break;

                case "UE":
                    match = "NotEqual";
                    break;

                case "GT":
                    match = "GreaterThan";
                    break;

                case "LT":
                    match = "LessThan";
                    break;

                case "IN":
                    match = "In";
                    break;

                case "FR":
                    match = "GreaterThanOrEqual";
                    break;

                case "TO":
                    match = "LessThanOrEqual";
                    break;

                case "LK":
                    match = "Like";
                    break;

                case "SW":
                    match = "StartsWith";
                    break;

                case "EW":
                    match = "EndsWith";
                    break;

                case "IGNORE":
                    match = "";
                    break;
            }
            return match;
        }

        /////     Binds the model to a value by using the specified controller context and binding context.
    }
}