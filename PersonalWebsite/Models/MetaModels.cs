using PersonalWebsite.Areas.Blog.Models;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalWebsite.Models
{
    public interface ITempable
    {
        [System.ComponentModel.DataAnnotations.Required]
        Dictionary<string, string> TempTokens { get; set; }
    }
    public class SimpleTempable : ITempable
    {
        public Dictionary<string, string> TempTokens { get; set; }
        public SimpleTempable()
        {
            TempTokens = new Dictionary<string, string>();
        }
    }
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
    public static class LinqExtensions
    {
        public static bool Contains<T>(this IEnumerable<T> enumerable, T value, Func<T, T, bool> comparer)
        {
            return enumerable.Contains(value, new FuncEqualityComparer<T>(comparer));
        }
        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> comparer)
        {
            return first.Intersect(second, new FuncEqualityComparer<T>(comparer));
        }
        private class FuncEqualityComparer<T> : IEqualityComparer<T>
        {
            readonly Func<T, T, bool> _comparer;
            readonly Func<T, int> _hash;
            // NB Cannot assume anything about how e.g., t.GetHashCode() interacts with the comparer's behavior
            public FuncEqualityComparer(Func<T, T, bool> comparer) : this(comparer, t => 0) {}
            public FuncEqualityComparer(Func<T, T, bool> comparer, Func<T, int> hash)
            { _comparer = comparer; _hash = hash; }
            public bool Equals(T x, T y) { return _comparer(x, y); }
            public int GetHashCode(T obj) { return _hash(obj); }
        }
    }
    public static class ControllerExtensions
    {
        public static TContext GetDb<TContext>(this Controller controller) where TContext : System.Data.Entity.DbContext
        {
            return controller.HttpContext.GetOwinContext().Get<TContext>();
        }
        public static ApplicationUserManager GetUserManager(this Controller controller)
        {
            return controller.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();//.Get<ApplicationUserManager>();
        }
        //public static async Task<ApplicationUser> GetAppUserAsync(this Controller controller)
        //{
        //    return await controller.GetUserManager().FindByIdAsync(controller.User.Identity.GetUserId());
        //}
        public static void AddTemp<T>(this Controller controller, ITempable tempable, string key, T value)
        {
            string token = Guid.NewGuid().ToString();
            tempable.TempTokens.Add(key, token);
            controller.TempData.Add(token, value);
        }
        public static T GetTemp<T>(this Controller controller, ITempable tempable, string key)
        {
            string token;
            if (!tempable.TempTokens.TryGetValue(key, out token))
                return default(T);
            object value = controller.TempData.Peek(token);
            if (value == null)
                return default(T);
            else if (value is T)
                return (T)value;
            else
                try { return (T)Convert.ChangeType(value, typeof(T)); }
                catch (InvalidCastException) { return default(T); }
        }
        public static void ClearTemp(this Controller controller, ITempable tempable)
        {
            controller.TempData.Clear();
            tempable.TempTokens.Clear();
        }
    }
    public static class ViewExtensions
    {
        public static ApplicationUserManager GetUserManager(this WebViewPage view)
        {
            return view.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }
        // Will return external user data by type
        public static string ExternalUserData(this WebViewPage view, ApplicationUser user, string claimType)
        {
            return user.Claims.FirstOrDefault(c => c.ClaimType == claimType).ClaimValue;
        }
        // Will return external user data by type
        public static string ExternalUserData(this WebViewPage view, System.Security.Principal.IIdentity user, string claimType)
        {
            return (user as System.Security.Claims.ClaimsIdentity).FindFirstValue(claimType);
        }
        // Will return the first external user data that matches the match
        public static string ExternalUserData(this WebViewPage view, System.Security.Principal.IIdentity user, Predicate<System.Security.Claims.Claim> match)
        {
            return (user as System.Security.Claims.ClaimsIdentity).FindFirst(match).Value;
        }
    }
    public static class AppBuilderExtensions
    {
        public static TOptions GetCredentials<TOptions,TProvider,TContext>(this Owin.IAppBuilder app, string key, System.Action<TContext> onAuthentication = null)
            where TOptions : Microsoft.Owin.Security.AuthenticationOptions, new()
            where TProvider : new() // Authentication Provider Type (Why is there no base class for this?)
            where TContext : Microsoft.Owin.Security.Provider.BaseContext
        {
            string[] creds = System.Configuration.ConfigurationManager.AppSettings[key].Split(',');
            dynamic options = new TOptions();
            try
            {
                if (onAuthentication != null) {
                    options.Provider = new TProvider();
                    options.Provider.OnAuthenticated = new Func<TContext, Task>(
                        async (context) => await Task.Run(() => onAuthentication(context)));
                }
                options.ClientId = creds[0];
                options.ClientSecret = creds[1];
                for (int i = 2; i < creds.Length; i++)
                    options.Scope.Add(creds[i]);
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) { Console.WriteLine(e.Message); }
            return options;
        }
    }
}