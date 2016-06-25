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
            return this.Ok(qb);
        }
    }
}
