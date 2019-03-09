using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRNLog.Providers
{
    public class AuthKey
    {
        public const string AllowOrigin = "client-AllowedOrigin";
        public const string RefreshTokenLifeTime = "client-RefreshTokenLifeTime";

        public class AuthProp
        {
            public const string AppId = "client-app-id";
            public const string SessionUserName = "userName";
        }

    }
}