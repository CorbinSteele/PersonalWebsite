using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PersonalWebsite.Areas.Blog.Models;

namespace PersonalWebsite.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostContent> PostContents { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentContent> CommentContents { get; set; }
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
    }
}