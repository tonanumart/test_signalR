using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestClientTest.Cls.Authen
{
    public interface ITokenManager
    {
        string Access_token { get; }
        string Token_type { get; }
        string Refresh_token { get; }
        void NotifyChangeToken(BearerToken token); 
       
    }
}
