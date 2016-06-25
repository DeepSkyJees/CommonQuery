# CommonQuery
Query for MVC and API Example Code

####1、Html Page
```sh
<form id="submitForm" method="POST">
    <input name='prefix' value='JYM_' type='hidden'>
    <input name="JYM_$EQ$Name"/>
    <input id="submit" type="button" value="Search"/>
</form>
```
</prev>
####2、JS Code
```sh
<script>
    $(function () {
        $("#submit").on("click", function () {
            $.post("@Url.Action("List")", $("#submitForm").serializeArray(), function (data) {
                $("#result").text(JSON.stringify(data));
            });
        });
    });
</script>
```
####3、C# Code
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
