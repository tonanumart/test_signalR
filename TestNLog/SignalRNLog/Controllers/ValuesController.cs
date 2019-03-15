﻿using NLog;
using SignalRNLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
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
            Thread.Sleep(rand.Next(5) * 1000);
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Authorize]
        public AuthViewModel Get(int id)
        {
            Thread.Sleep(rand.Next(10) * 1000);
            var claimsIdentity = User.Identity as ClaimsIdentity;;
            return new AuthViewModel
            {
                Name = User.Identity.Name,
                AuthType = User.Identity.AuthenticationType,
                IsAuth = User.Identity.IsAuthenticated,
                Times = claimsIdentity.FindFirst("claimTimes")
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


   
}