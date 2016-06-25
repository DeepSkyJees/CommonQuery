using CommonQuery.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CommonQuery.Api.Example.Controllers
{
    public class DefaultController : ApiController
    {
        public IHttpActionResult Post(BaseQueryBuilder qb)
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
