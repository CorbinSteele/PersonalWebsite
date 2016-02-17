using System.Web.Mvc;

namespace PersonalWebsite.Areas.CarFinder.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Car Finder";

            return View();
        }
    }
}
