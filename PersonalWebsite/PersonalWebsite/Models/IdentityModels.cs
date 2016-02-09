using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PersonalWebsite.Areas.Blog.Models;
using System.Linq;
using System.Collections.Generic;

namespace PersonalWebsite.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, Task<ExternalLoginInfo> loginInfoTask)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType;
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            if (loginInfoTask == null)
                return userIdentity;
            ExternalLoginInfo loginInfo = await loginInfoTask;
            if (loginInfo == null)
                return userIdentity;
            // Add custom user claims here
            List<Claim> newClaims = new List<Claim>() { new Claim("urn:" + loginInfo.Login.LoginProvider.ToLower() + ":emailaddress", loginInfo.Email) };
            newClaims.AddRange(loginInfo.ExternalIdentity.Claims.Where((c) => !c.Type.StartsWith("http")));
            var existingClaims = await manager.GetClaimsAsync(userIdentity.GetUserId());
            // This ToList forces an eager evaluation, therefore it is very important!
            existingClaims.Intersect(newClaims, (c1, c2) => c1.Type == c2.Type && c1.Value != c2.Value)
                          .Select((c) => manager.RemoveClaim(userIdentity.GetUserId(), c)).ToList();
            newClaims.Where((c) => !existingClaims.Contains(c, (ec, nc) => ec.Type == nc.Type && ec.Value == nc.Value))
                     .Select((c) => manager.AddClaim(userIdentity.GetUserId(), c)).ToList();
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Comment>().
              HasOptional(c => c.Parent).
              WithMany(c => c.Comments).
              HasForeignKey(c => c.ParentCommentID);
        }
    }
}