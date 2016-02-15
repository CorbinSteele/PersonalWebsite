using System.Web.Mvc;

namespace PersonalWebsite.Areas.Budgeter
{
    public class BudgeterAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Budgeter";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Budgeter_Default",
                "Budgeter/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new [] { "PersonalWebsite.Areas.Budgeter.Controllers" }
            );
        }
    }
}