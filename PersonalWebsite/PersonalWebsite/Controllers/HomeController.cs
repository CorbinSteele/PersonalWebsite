using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PersonalWebsite.Models;
namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*if (User.Identity.IsAuthenticated)
            {
                var x = this.GetAppUserAsync();
                var result = x.Claims.SingleOrDefault((c) => c.ClaimType == "urn:microsoft:emailaddress").ClaimValue;
            }*/
            return View();
        }
    }
}