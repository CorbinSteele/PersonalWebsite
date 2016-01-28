using System.Web.Mvc;

namespace PersonalWebsite.Areas.Blog
{
    public class BlogAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Blog";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Blog_default",
                "Blog/{controller}/{action}/{id}",
                new { controller = "Posts", action = "Index", id = UrlParameter.Optional },
                new [] { "PersonalWebsite.Areas.Blog.Controllers" }
            );
            context.MapRoute(
                "Blog_Post",
                "Blog/Post/{id}/{controller}/{action}",
                new { controller = "Posts", action = "Details", id = UrlParameter.Optional },
                new[] { "PersonalWebsite.Areas.Blog.Controllers" }
            );
        }
    }
}