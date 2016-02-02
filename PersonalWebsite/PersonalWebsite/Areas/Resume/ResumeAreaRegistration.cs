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
                "Resume",
                new { controller = "Home", action = "Index" },
                new[] { "PersonalWebsite.Areas.Resume.Controllers" }
            );
        }
    }
}