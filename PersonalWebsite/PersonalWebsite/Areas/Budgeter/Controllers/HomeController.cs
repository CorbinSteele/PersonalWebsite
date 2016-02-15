using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalWebsite.Areas.Blog.Models;
using PersonalWebsite.Models;
using Microsoft.AspNet.Identity;

namespace PersonalWebsite.Areas.Budgeter.Controllers
{
    public class HomeController : Controller
    {
        // GET: Blog/Posts
        public async Task<ActionResult> Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}
