namespace PersonalWebsite.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PersonalWebsite.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PersonalWebsite.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PersonalWebsite.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole { Name = "Admin" });
            if (!roleManager.RoleExists("Moderator"))
                roleManager.Create(new IdentityRole { Name = "Moderator" });

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByName("Corbin Steele");
            /*if (user == null)
            {
                var newUser = userManager.Create(new ApplicationUser() { UserName = "Corbin Steele" }, "password");
                userManager.AddToRole(userManager.FindByName("Corbin Steele").Id, "Admin");
            }*/
        }
    }
}
