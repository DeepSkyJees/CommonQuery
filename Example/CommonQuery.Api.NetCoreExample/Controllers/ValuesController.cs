using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonQuery.Builder;
using Microsoft.AspNetCore.Mvc;

namespace CommonQuery.Api.NetCoreExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpPost]
        public ActionResult<IEnumerable<string>> Post(BaseQueryBuilder qb)
        {

            IQueryable<User> users = new User().GetUsers().Where(qb);
            return this.Ok(users);
        }


    }


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
