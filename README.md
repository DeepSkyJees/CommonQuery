# CommonQuery
Query for MVC and API Example Code

####1、Html Page
```sh
<form id="submitForm" method="POST">
    <input name='prefix' value='JYM_' type='hidden'>
    <input name="$EQ$Name"/>
    <input id="submit" type="button" value="Search"/>
</form>
```
</prev>
####2、C# Code
```sh
public ActionResult List(QueryBuilder qbBuilder)
{
    using (Models.Models contextModels = new Models.Models())
    {
        var result = contextModels.MyEntities.Where(qbBuilder).ToList();
         return this.Json(result);
    }
}
```
