using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC_WEBAPI_Project.Controllers
{
    public class WEBAPICController : ApiController
    {
        // GET: api/WEBAPIC
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/WEBAPIC/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WEBAPIC
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/WEBAPIC/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/WEBAPIC/5
        public void Delete(int id)
        {
        }
    }
}
