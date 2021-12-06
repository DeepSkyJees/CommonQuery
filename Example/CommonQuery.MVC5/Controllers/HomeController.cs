
using CommonQuery.Builder;
using CommonQuery.MVC.Core;
using CommonQuery.MVC5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CommonQuery.MVC5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        ///     Lists the specified qb builder.
        /// </summary>
        /// <param name="qbBuilder">The qb builder.</param>
        /// <returns>ActionResult.</returns>
        public async Task<IActionResult> List(QueryBuilder qbBuilder)
        {
            using (MyContext contextModels = new MyContext())
            {
                await contextModels.Database.MigrateAsync();
                if (!contextModels.MyEntities.Any(p => p.Id == 1))
                {
                    await contextModels.MyEntities.AddAsync(new MyEntity
                    {
                        Id = 1,
                        Name = "Jeeees"
                    });
                    await contextModels.SaveChangesAsync();
                }
                var result = contextModels.MyEntities.Where(qbBuilder).ToList();
                return this.Json(result);
            }
        }
    }
}
