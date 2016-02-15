using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using PersonalWebsite.Areas.Blog.Models;
using PersonalWebsite.Areas.Budgeter.Models;

namespace PersonalWebsite.Models
{
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserDbContext() : base("DefaultConnection", throwIfV1Schema: false) { }
        public static UserDbContext Create()
        {
            return new UserDbContext();
        }
    }
    public class CarFinderDbContext : DbContext
    {
        public CarFinderDbContext() : base("CarFinderConnection") { }
        public static CarFinderDbContext Create()
        {
            return new CarFinderDbContext();
        }
    }
    public class BlogDbContext : DbContext
    {
        public BlogDbContext() : base("BlogConnection") { }
        public static BlogDbContext Create()
        {
            return new BlogDbContext();
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
    public class BudgeterDbContext : DbContext
    {
        public BudgeterDbContext() : base("BudgeterConnection") { }
        public static BudgeterDbContext Create()
        {
            return new BudgeterDbContext();
        }

        public DbSet<Household> Households { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
    }
}