using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClientTest.Cls.Authen
{
    public interface ISmartkorpAuthRequest
    {
        AuthMode getModeAuthen();
        string GetRefreshToken();
        SmartkorpAuthenForm GetUserNamePassword();
    }
}
