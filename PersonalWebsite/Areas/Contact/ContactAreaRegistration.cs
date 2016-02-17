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
                "Contact",
                new { controller = "Home", action = "Index" },
                new[] { "PersonalWebsite.Areas.Contact.Controllers" }
            );
        }
    }
}