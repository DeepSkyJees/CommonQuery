using CommonQuery.Builder;
using System.Linq;
using System.Web.Mvc;

namespace CommonQuery.MVC.Example.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        ///     Lists the specified qb builder.
        /// </summary>
        /// <param name="qbBuilder">The qb builder.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult List(QueryBuilder qbBuilder)
        {
            using (Models.Models contextModels = new Models.Models())
            {
                var result = contextModels.MyEntities.Where(qbBuilder).ToList();
                return this.Json(result);
            }
        }
    }
}