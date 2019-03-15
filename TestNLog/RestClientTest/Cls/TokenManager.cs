using RestClientTest.Cls.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestClientTest.Cls
{
    public class TokenManager : ITokenManager
    {
        //public static object refreshToken = new object();
        public BearerToken bearerToken { get; set; }
        public TokenManager()
        {
            bearerToken = new BearerToken();
        }

        public string Access_token
        {
            get { return bearerToken.access_token; }
        }

        public string Token_type
        {
            get { return bearerToken.token_type; }
        }

        public string Refresh_token
        {
            get { return bearerToken.refresh_token; }
        }


        public void NotifyChangeToken(BearerToken token)
        {
            this.bearerToken = token;
        }



    }
}
