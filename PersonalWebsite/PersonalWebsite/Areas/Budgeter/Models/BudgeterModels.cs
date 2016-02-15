using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Areas.Budgeter.Models
{
    public class Household
    {
        public Household()
        {
            this.UserHouseholds = new HashSet<UserHousehold>();
        }

        public int HouseholdID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserHousehold> UserHouseholds { get; set; }
    }
    public class UserHousehold
    {
        public UserHousehold()
        {
            this.Invitations = new HashSet<Invitation>();
        }
        public int UserHouseholdID { get; set; }
        public string UserID { get; set; }
        public int HouseholdID { get; set; }

        public ApplicationUser GetUser(ApplicationUserManager userManager)
        {
            return userManager.FindUserOrDefault(UserID).Result;
        }
        public virtual Household Household { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
    }
    public class Account
    {
        public int AccountID { get; set; }
        public int HouseholdID { get; set; }
        public string Name { get; set; }
        public double ReconciledBalance { get; set; }

        public Household Household { get; set; }
    }
    public class Category
    {
        public enum Persistences
        {
            Monthly, Quarterly, Yearly, Once
        }
        public Category()
        {
            this.Transactions = new HashSet<Transaction>();
        }
        public int CategoryID { get; set; }
        public int AccountID { get; set; }
        public string UpdatedByID { get; set; }
        // Always lasts one month in duration
        public DateTimeOffset StartDate { get; set; }
        public string Title { get; set; }
        public double Amount { get; set; }
        public Persistences Persistence { get; set; }
        // This is decremented every time the category persists, once 0 it ceases to persist; -1 is endless persistence
        public int PersistentLimit { get; set; }

        public ApplicationUser GetUpdatedBy(ApplicationUserManager userManager)
        {
            return userManager.FindUserOrDefault(UpdatedByID).Result;
        }
        public virtual Account Account { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int CategoryID { get; set; }
        public DateTimeOffset Date { get; set; }
        public string UpdatedByID { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double ReconciledAmount { get; set; }

        public ApplicationUser GetUpdatedBy(ApplicationUserManager userManager)
        {
            return userManager.FindUserOrDefault(UpdatedByID).Result;
        }
        public virtual Category Category { get; set; }
    }
    public class Invitation
    {
        public int InvitationID { get; set; }
        public int UserHouseholdID { get; set; }
        public string ToEmail { get; set; }

        public virtual UserHousehold UserHousehold { get; set; }
    }
}