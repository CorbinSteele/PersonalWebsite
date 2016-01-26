using System.Web.Mvc;

namespace PersonalWebsite.Areas.Resume
{
    public class ResumeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Resume";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Resume_default",
                "Resume/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "PersonalWebsite.Areas.Resume.Controllers" }
            );
        }
    }
}