using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace SignalRNLog.Controllers
{
    public class ValuesController : ApiController
    {
        public static Random rand = new Random();
        // GET api/values
        public IEnumerable<string> Get()
        {
            Thread.Sleep(rand.Next(4) * 500);
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Authorize]
        public MyClass Get(int id)
        {
            return new MyClass
            {
                Name = User.Identity.Name,
                AuthType = User.Identity.AuthenticationType,
                IsAuth = User.Identity.IsAuthenticated
            };
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }


    public class MyClass
    {

        public string Name { get; set; }

        public string AuthType { get; set; }

        public bool IsAuth { get; set; }
    }
}