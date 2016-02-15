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
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            ApplicationUser currentUser = await this.GetUserManager().FindByNameAsync(User.Identity.Name);
            if (currentUser != null && (/*DELETE_USER =*/ false))
            {
                this.HttpContext.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
                var result = await this.GetUserManager().DeleteAsync(currentUser);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}