using System.Web.Mvc;

namespace PersonalWebsite.Areas.About
{
    public class AboutAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "About";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "About_default",
                "About",
                new { controller = "Home", action = "Index" },
                new[] { "PersonalWebsite.Areas.About.Controllers" }
            );
        }
    }
}