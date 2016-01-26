using System.Web.Http;
using System.Web.Mvc;

namespace PersonalWebsite.Areas.CarFinder
{
    public class CarFinderAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CarFinder";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CarFinder_Default",
                url: "CarFinder/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "PersonalWebsite.Areas.CarFinder.Controllers" }
            );
        }
    }
}