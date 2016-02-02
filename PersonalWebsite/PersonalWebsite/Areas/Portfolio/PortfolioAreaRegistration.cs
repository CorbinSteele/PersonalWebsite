using System.Web.Mvc;

namespace PersonalWebsite.Areas.Portfolio
{
    public class PortfolioAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Portfolio";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Portfolio_default",
                "Portfolio",
                new { controller = "Home", action = "Index" },
                new[] { "PersonalWebsite.Areas.Portfolio.Controllers" }
            );
        }
    }
}