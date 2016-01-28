using PersonalWebsite.Areas.Portfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalWebsite.Areas.Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private List<Project> projects = new List<Project>() {
            new Project("Javascript Exercises", "js", "_JsPartial", "/Areas/Portfolio/images/javascript.png",
                "Multiple small Javascript exercises, all with source code and simple interfaces. " +
                "The exercises include multiple examples of complex logic expressions, " +
                "looping constructs, string and number manipulation, creative use of anonymous objects and functions, " +
                "extensive use of the JQuery and Javascript libraries, and even one particularly interesting use of Infinity. " +
                "There are only four exercises, so it won't cost too much time to check them all out!"),
            new Project("Car Finder", "cf", "_CfPartial", "/images/360-temp.png", "A project that finds car data.")
        };

        // GET: About/Home
        public ActionResult Index()
        {
            return View(projects);
        }
    }
}