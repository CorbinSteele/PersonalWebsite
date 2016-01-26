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
                "Portfolio/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "PersonalWebsite.Areas.Portfolio.Controllers" }
            );
        }
    }
}