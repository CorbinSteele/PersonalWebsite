using PersonalWebsite.Areas.Budgeter.Models;
using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace PersonalWebsite.Areas.Budgeter.Controllers
{
    public class HouseholdsController : Controller
    {
        private BudgeterDbContext GetDb() { return this.GetDb<BudgeterDbContext>(); }

        //TEMP
        private const bool DEBUG = true;
        private const string USER_ID_DEBUG = "e1a47aaa-abe2-4b56-b0ac-baad239ae49c";

        // POST: Budgeter/Households
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult> Index()
        {
            string userId = User.Identity.GetUserId();
            IList<Household> households = await GetDb().Households.Include("UserHouseholds").Where(h => h.UserHouseholds.Any(uh => uh.UserID == (DEBUG ? USER_ID_DEBUG : userId))).ToListAsync();
            // Nesting inside a 'json' attribute is necessary to avoid third-party scripting attacks
            return Json(households.Select(h => new {
                householdId = h.HouseholdID,
                userHouseholdId = h.UserHouseholds.First(uh => uh.UserID == (DEBUG ? USER_ID_DEBUG : userId)).UserHouseholdID,
                name = h.Name,
                users = h.GetMembers(this.GetUserManager()).Select(u => new {
                    id = u.Id,
                    name = u.Claims.FirstOrDefault((c) => !c.ClaimType.StartsWith("http") && c.ClaimType.EndsWith("name")).ClaimValue,
                    email = u.Email,
                    avatar = u.Claims.FirstOrDefault(uc => uc.ClaimType == "urn:github:avatar").ClaimValue
                }),
                accounts = GetDb().Accounts.Where(a => h.HouseholdID == a.HouseholdID)
            }));
        }

        // POST: Budgeter/Households/Create
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult> Create(HouseholdView householdView)
        {
            if (ModelState.IsValid)
            {
                Household household = new Household();
                household.Name = householdView.Name;
                household = GetDb().Households.Add(household);
                UserHousehold userHousehold = new UserHousehold();
                userHousehold.UserID = DEBUG ? USER_ID_DEBUG : User.Identity.GetUserId();
                household.UserHouseholds.Add(userHousehold);
                await GetDb().SaveChangesAsync();

                return Json(new { result = "Success" });
            }
            return Json(new { result = "Failed" });
        }

        // POST: Budgeter/Households/Edit
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Edit(HouseholdView householdView)
        {
            if (ModelState.IsValid)
            {
                Household household = await GetDb().Households.FindAsync(householdView.HouseholdID);
                household.Name = householdView.Name;
                await GetDb().SaveChangesAsync();

                return Json(new { result = "Success" });
            }
            return Json(new { result = "Failed" });
        }

        // GET: Budgeter/Households/InviteMember/3
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult> InviteMember(int? id)
        {
            if (id == null)
                return HttpNotFound();
            UserHousehold userHousehold = await GetDb().UserHouseholds.FindAsync(id);
            if (userHousehold == null || userHousehold.UserID != (DEBUG ? USER_ID_DEBUG : User.Identity.GetUserId()))
                return HttpNotFound();
            ViewBag.Title = "Invite New Member to " + userHousehold.Household.Name;
            InviteMemberView invitationView = new InviteMemberView();
            this.AddTemp(invitationView, "UserHouseholdID", id.GetValueOrDefault());
            return PartialView("_InviteMemberPartial", invitationView);
        }

        // POST: Budgeter/Households/InviteMember
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult> InviteMember(InviteMemberView inviteMemberView)
        {
            if (ModelState.IsValid)
            {
                Invitation invitation = new Invitation();
                invitation.ToEmail = inviteMemberView.Email;
                invitation.UserHouseholdID = this.GetTemp<int>(inviteMemberView, "UserHouseholdID");
                GetDb().Invitations.Add(invitation);
                await GetDb().SaveChangesAsync();

                return Json(new { result = "Success" });
            }
            return Json(new { result = "Failed" });
        }
    }
}