using System.Web.Mvc;

namespace PersonalWebsite.Areas.ContactMe
{
    public class ContactMeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ContactMe";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ContactMe_default",
                "ContactMe/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "PersonalWebsite.Areas.ContactMe.Controllers" }
            );
        }
    }
}