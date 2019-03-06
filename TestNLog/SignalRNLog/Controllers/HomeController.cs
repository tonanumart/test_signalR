using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRNLog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Name = User.Identity.Name;
            ViewBag.AuthType = User.Identity.AuthenticationType;
            ViewBag.IsAuth = User.Identity.IsAuthenticated;
            return View();
        }

        [Authorize]
        public ActionResult MVCLogin()
        {
            ViewBag.Name = User.Identity.Name;
            ViewBag.AuthType = User.Identity.AuthenticationType;
            ViewBag.IsAuth = User.Identity.IsAuthenticated;
            return View();
        }
         
        public ActionResult TestLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return RedirectToAction("Index");


            SessionContext context = new SessionContext();
            context.SetAuthenticationToken(username, false, new User() {  FirstName = "Anumart" , LastName = "Chaichana" });

            return RedirectToAction("MVCLogin");
        }
    }
}
