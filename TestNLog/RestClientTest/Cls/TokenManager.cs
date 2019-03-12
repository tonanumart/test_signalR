using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestClientTest.Cls
{
    public class TokenManager
    {
        //public static object refreshToken = new object();
        public BearerToken bearerToken { get; set; }
        public TokenManager()
        {
            bearerToken = new BearerToken();
        }
    }
}
