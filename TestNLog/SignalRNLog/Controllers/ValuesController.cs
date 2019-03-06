using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SignalRNLog.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Sample informational message in api/values");
            var test = 0;
            var result = 133 / test;
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