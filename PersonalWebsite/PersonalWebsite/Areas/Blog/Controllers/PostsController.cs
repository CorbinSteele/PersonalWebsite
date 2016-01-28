using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PersonalWebsite.Areas.Blog.Models;
using PersonalWebsite.Models;

namespace PersonalWebsite.Areas.Blog.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Blog/Posts
        public async Task<ActionResult> Index()
        {
            return View((await this.GetDb().Posts.ToListAsync()));
        }

        // GET: Blog/Posts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await this.GetDb().Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Blog/Posts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Title,Extract,Content,DoPublish")] PostView postView)
        {
            if (ModelState.IsValid)
            {
                Post post = new Post();
                post.AuthorID = (await this.GetAppUserAsync()).Id;
                post.Title = postView.Title;
                if (postView.DoPublish)
                    post.CreatedOn = new DateTimeOffset(DateTime.Now);
                this.GetDb().Posts.Add(post);
                await this.GetDb().SaveChangesAsync();
                post = await this.GetDb().Posts.FirstAsync(p => p.Title == post.Title);

                PostContent postContent = new PostContent();
                postContent.PostID = post.PostID;
                postContent.Extract = postView.Extract;
                postContent.Content = postView.Content;
                this.GetDb().PostContents.Add(postContent);
                await this.GetDb().SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(postView);
        }

        // GET: Blog/Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            Post post = await this.GetDb().Posts.FirstOrDefaultAsync(p => p.Title == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            EditedPostView postView = new EditedPostView();
            postView.DoPublish = post.CreatedOn != null;
            PostContent postContent = post.Content;
            postView.Extract = postContent.Extract;
            postView.Content = postContent.Content;
            postView.IsDeleted = postContent.IsDeleted;
            postView.UpdateReason = postContent.UpdateReason;
            this.AddTemp(postView, "PostID", post.PostID);
            return View(postView);
        }

        // POST: Blog/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "TempTokens,Extract,Content,DoPublish,UpdatedReason,IsDeleted")] EditedPostView postView)
        {
            Post post = await this.GetDb().Posts.FindAsync(this.GetTemp<int>(postView, "PostID"));//.FirstAsync(p => p.Title == postView.Title);
            if (post == null)
            {
                this.ClearTemp(postView);
                return RedirectToAction("Index");
            }
            ModelState value;
            if (ModelState.TryGetValue("Title", out value))
                value.Errors.Clear();
            if (ModelState.IsValid && (postView.DoPublish || post.CreatedOn == null))
            {
                PostContent postContent = await this.GetDb().PostContents.FirstAsync(pc => pc.PostID == post.PostID);
                if (!postView.DoPublish) // This is a draft that needs to not be published
                {
                    postContent.Extract = postView.Extract;
                    postContent.Content = postView.Content;
                    this.GetDb().Entry(postContent).State = EntityState.Modified;
                }
                else if (post.CreatedOn == null) // This is an edited draft
                {
                    if (postView.IsDeleted) // The draft needs to be deleted
                    {
                        this.GetDb().PostContents.Remove(postContent);
                        this.GetDb().Posts.Remove(post);
                    }
                    else // The draft needs to be published
                    {
                        post.CreatedOn = new DateTimeOffset(DateTime.Now);
                        this.GetDb().Entry(post).State = EntityState.Modified;
                        postContent.Extract = postView.Extract;
                        postContent.Content = postView.Content;
                        this.GetDb().Entry(postContent).State = EntityState.Modified;
                    }
                }
                else // This is a pre-existing post
                {
                    postContent = new PostContent();
                    postContent.PostID = post.PostID;
                    postContent.EditorID = (await this.GetUserManager().FindByNameAsync(User.Identity.Name)).Id;
                    postContent.UpdatedOn = new DateTimeOffset(DateTime.Now);
                    postContent.UpdateReason = postView.UpdateReason;
                    postContent.Extract = postView.Extract;
                    postContent.Content = postView.Content;
                    postContent.IsDeleted = postView.IsDeleted;
                    this.GetDb().PostContents.Add(postContent);
                }
                this.ClearTemp(postView);
                await this.GetDb().SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (postView.DoPublish && post.CreatedOn == null)
                postView.DoPublish = false;
            return View(postView);
        }
        /*
        // GET: Blog/Posts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await this.GetDb().Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Blog/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Post post = await this.GetDb().Posts.FindAsync(id);
            this.GetDb().Posts.Remove(post);
            await this.GetDb().SaveChangesAsync();
            return RedirectToAction("Index");
        }
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
