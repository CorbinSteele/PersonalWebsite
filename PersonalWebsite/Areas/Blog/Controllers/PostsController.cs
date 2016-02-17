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
using Microsoft.AspNet.Identity;

namespace PersonalWebsite.Areas.Blog.Controllers
{
    public class PostsController : Controller
    {
        private BlogDbContext GetDb() { return this.GetDb<BlogDbContext>(); }

        // GET: Blog/Posts
        public async Task<ActionResult> Index()
        {
            return View(await GetDb().Posts.ToListAsync());
        }

        // GET: Blog/Posts/Details/5
        // GET: Blog/Post/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await GetDb().Posts.FindAsync(id);
            if (post == null || (post.CreatedOn == null && post.AuthorID != User.Identity.GetUserId()))
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
                post.AuthorID = User.Identity.GetUserId();
                post.Title = postView.Title;
                if (postView.DoPublish)
                    post.CreatedOn = new DateTimeOffset(DateTime.Now);
                post = GetDb().Posts.Add(post);
                await GetDb().SaveChangesAsync();

                PostContent postContent = new PostContent();
                postContent.PostID = post.PostID;
                postContent.Extract = postView.Extract;
                postContent.Content = postView.Content;
                GetDb().PostContents.Add(postContent);
                await GetDb().SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(postView);
        }

        // GET: Blog/Posts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await GetDb().Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            EditedPostView postView = new EditedPostView(post);
            this.AddTemp(postView, "PostID", post.PostID);
            return View(postView);
        }

        // POST: Blog/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "TempTokens,Title,Extract,Content,DoPublish,UpdateReason,IsDeleted")] EditedPostView postView)
        {
            Post post = await GetDb().Posts.FindAsync(this.GetTemp<int>(postView, "PostID"));//.FirstAsync(p => p.Title == postView.Title);
            if (post == null)
            {
                this.ClearTemp(postView);
                return RedirectToAction("Index");
            }
            /*ModelState value;
            if (ModelState.TryGetValue("Title", out value))
                value.Errors.Clear();*/
            if (ModelState.IsValid && (postView.DoPublish || post.CreatedOn == null))
            {
                PostContent postContent = await GetDb().PostContents.FirstAsync(pc => pc.PostID == post.PostID);
                if (!postView.DoPublish) // This is a draft that needs to not be published
                {
                    postContent.Extract = postView.Extract;
                    postContent.Content = postView.Content;
                    GetDb().Entry(postContent).State = EntityState.Modified;
                }
                else if (post.CreatedOn == null) // This is an edited draft
                {
                    if (postView.IsDeleted) // The draft needs to be deleted
                    {
                        GetDb().PostContents.Remove(postContent);
                        GetDb().Posts.Remove(post);
                    }
                    else // The draft needs to be published
                    {
                        post.CreatedOn = new DateTimeOffset(DateTime.Now);
                        GetDb().Entry(post).State = EntityState.Modified;
                        postContent.Extract = postView.Extract;
                        postContent.Content = postView.Content;
                        GetDb().Entry(postContent).State = EntityState.Modified;
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
                    GetDb().PostContents.Add(postContent);
                }
                this.ClearTemp(postView);
                await GetDb().SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (postView.DoPublish && post.CreatedOn == null)
                postView.DoPublish = false;
            postView.Title = post.Title;
            return View(postView);
        }
        // GET: Blog/Posts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = await GetDb().Posts.FindAsync(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var deleteView = new EditedPostView(post);
            this.AddTemp(deleteView, "PostID", post.PostID);
            return View(deleteView);
        }

        // POST: Blog/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed([Bind(Include = "TempTokens,UpdateReason")] EditedPostView postView)
        {
            Post post = await GetDb().Posts.FindAsync(this.GetTemp<int>(postView, "PostID"));
            if (post.Content.IsDeleted) // It was previously deleted
                return RedirectToAction("Index");
            if (post.CreatedOn == null) // It's a draft
            {
                GetDb().PostContents.Remove(post.Content);
                GetDb().Posts.Remove(post);
            }
            else // It's a previously published post
            {
                PostContent postContent = new PostContent();
                postContent.PostID = post.PostID;
                postContent.EditorID = (await this.GetUserManager().FindByNameAsync(User.Identity.Name)).Id;
                postContent.UpdatedOn = new DateTimeOffset(DateTime.Now);
                postContent.UpdateReason = postView.UpdateReason;
                postContent.Extract = post.Content.Extract;
                postContent.Content = post.Content.Content;
                postContent.IsDeleted = true;
                GetDb().PostContents.Add(postContent);
            }
            await GetDb().SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}
