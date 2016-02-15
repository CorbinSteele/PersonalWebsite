namespace PersonalWebsite.Migrations.UserMigrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PersonalWebsite.Models.UserDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\UserMigrations";
        }

        protected override void Seed(PersonalWebsite.Models.UserDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole { Name = "Admin" });
            if (!roleManager.RoleExists("Moderator"))
                roleManager.Create(new IdentityRole { Name = "Moderator" });

            var user = context.Users.FirstOrDefault(u => u.UserName == "CorbinSteele");
            var role = context.Roles.FirstOrDefault(r => r.Name == "Admin");
            if (user != null && role.Users.All(ur => ur.UserId != user.Id))
                user.Roles.Add(new IdentityUserRole() { UserId = user.Id, RoleId = role.Id });

            context.SaveChanges();
        }
    }
}
