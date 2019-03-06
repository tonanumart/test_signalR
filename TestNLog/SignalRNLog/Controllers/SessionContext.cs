using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SignalRNLog.Controllers
{
    public class SessionContext
    {
        public void SetAuthenticationToken(string name, bool isPersistant, User userData)
        {
            string data = null;
            if (userData != null)
                data = new JavaScriptSerializer().Serialize(userData);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, name, DateTime.Now, DateTime.Now.AddMinutes(1), isPersistant, "userIdStringFormAuthenTicket");

            string cookieData = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieData)
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            }; 
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
