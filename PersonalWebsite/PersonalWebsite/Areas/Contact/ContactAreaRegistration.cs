using System.Web.Mvc;

namespace PersonalWebsite.Areas.Contact
{
    public class ContactAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Contact";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Contact_default",
                "Contact/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "PersonalWebsite.Areas.Contact.Controllers" }
            );
        }
    }
}