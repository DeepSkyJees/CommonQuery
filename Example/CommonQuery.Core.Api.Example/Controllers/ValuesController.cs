using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonQuery.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Runtime;
using System.Reflection;

namespace CommonQuery.Core.Api.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(BaseQueryBuilder qb)
        {
            var methods = typeof(string).GetMethods();
            MethodInfo methodInfo = methods.FirstOrDefault(p => p.Name == "Contains");
            IQueryable<User> users = new User().GetUsers().Where(qb);
            return this.Ok(users);
        }
       
    }
    // GET api/values
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IQueryable<User> GetUsers()
        {
            return new List<User>
            {
                new User {Id = 1,Name = "Nigel"},
                new User {Id=2,Name = "Jim"}
            }.AsQueryable();
        }
    }
}
