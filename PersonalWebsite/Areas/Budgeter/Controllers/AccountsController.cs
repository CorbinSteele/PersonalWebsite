using PersonalWebsite.Areas.Budgeter.Models;
using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace PersonalWebsite.Areas.Budgeter.Controllers
{
    public class AccountsController : Controller
    {
        private BudgeterDbContext GetDb() { return this.GetDb<BudgeterDbContext>(); }

        // POST: Budgeter/api/Accounts
        [HttpPost]
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        // GET: Budgeter/api/Accounts/Create/3
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Household household = await GetDb().Households.FindAsync(id);
            if (household == null || !household.HasMemberById(User.Identity.GetUserId()))
                return HttpNotFound();
            ViewBag.Title = "Add an account to " + household.Name;
            AddAccountView addAccountView = new AddAccountView();
            this.AddTemp(addAccountView, "HouseholdID", id.GetValueOrDefault());
            return PartialView("_AddAccountPartial", addAccountView);
        }

        // POST: Budgeter/api/Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(AddAccountView addAccountView)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account();
                account.Name = addAccountView.Name;
                account.ReconciledBalance = addAccountView.Balance;
                account.HouseholdID = this.GetTemp<int>(addAccountView, "HouseholdID");
                GetDb().Accounts.Add(account);
                await GetDb().SaveChangesAsync();

                return Json(new { result = "Success" });
            }
            return Json(new { result = "Failed" });
        }

        // POST: Budgeter/api/Accounts/Details/3
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return Json(new { });
            Account account = await GetDb().Accounts.FindAsync(id.GetValueOrDefault());
            if (account == null || !account.Household.HasMemberById(User.Identity.GetUserId()))
                return Json(new { });
            // TODO: Convert to STORED PROCEDURE
            return Json(new
            {
                id = account.AccountID,
                name = account.Name,
                balance = account.ReconciledBalance,
                transactions = GetDb().Transactions.Where(t => account.Categories.Contains(t.Category))//account.Categories.Aggregate(new List<Transaction>(), (ts, c) => { ts.AddRange(c.Transactions); return ts; })
            });
        }

    }
}