using PersonalWebsite.Areas.Blog.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace PersonalWebsite.Models
{
    public class ControllerAction
    {
        public enum Sizes { Small, Medium, Large }
        private Sizes displaySize;
        public string DisplaySize
        {
            get
            {
                switch (displaySize)
                {
                    case Sizes.Small: return "sm";
                    case Sizes.Medium: return "md";
                    case Sizes.Large: return "lg";
                    default: return "sm";
                }
            }
        }
        public string Controller { get; private set; }
        public string Action { get; private set; }
        public int? Id { get; private set; }
        public ControllerAction(string controller, string action, Sizes displaySize = Sizes.Small, int? id = null)
        {
            this.Controller = controller;
            this.Action = action;
            this.Id = id;
            this.displaySize = displaySize;
        }
    }
    public static class ControllerExtensions
    {
        public static ApplicationDbContext GetDb(this Controller controller)
        {
            return controller.HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        }
        public static ApplicationUserManager GetUserManager(this Controller controller)
        {
            return controller.HttpContext.GetOwinContext().Get<ApplicationUserManager>();
        }
        public async static Task<ApplicationUser> GetAppUserAsync(this Controller controller)
        {
            return await controller.GetUserManager().FindByNameAsync(controller.User.Identity.Name);
        }
        public static void AddTemp<T>(this Controller controller, ITempable tempable, string key, T value)
        {
            string token = System.Guid.NewGuid().ToString();
            tempable.TempTokens.Add(key, token);
            controller.TempData.Add(token, value);
        }
        public static T GetTemp<T>(this Controller controller, ITempable tempable, string key)
        {
            string token;
            object value;
            if (!tempable.TempTokens.TryGetValue(key, out token) || !controller.TempData.TryGetValue(token, out value))
                return default(T);
            if (value is T)
                return (T)value;
            else
                try { return (T)System.Convert.ChangeType(value, typeof(T)); }
                catch (System.InvalidCastException) { return default(T); }
        }
        public static void ClearTemp(this Controller controller, ITempable tempable)
        {
            controller.TempData.Clear();
            tempable.TempTokens.Clear();
        }
        public static string ViewAsContent(this Controller controller, string viewName, object model)
        {
            controller.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            controller.ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}