using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestClientTest.Cls
{
    public class CustomAuthenticator : IAuthenticator
    {
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
