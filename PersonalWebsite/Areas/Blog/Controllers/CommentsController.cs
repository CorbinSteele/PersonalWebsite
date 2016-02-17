using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PersonalWebsite.Areas.Blog.Models;
using PersonalWebsite.Models;

namespace PersonalWebsite.Areas.Blog.Controllers
{
    public class CommentsController : Controller
    {
        private BlogDbContext GetDb() { return this.GetDb<BlogDbContext>(); }

        // GET: Blog/Comments/Details/5
        [Authorize(Roles="Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await GetDb().Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Blog/Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include = "PostID,ParentCommentID,Content")] CommentView commentView)
        {
            string userId = User.Identity.GetUserId();
            if (ModelState.IsValid && await GetDb().Posts.AnyAsync(p =>
                    p.PostID == commentView.PostID && (p.CreatedOn != null || p.AuthorID == userId))
               &&
               (commentView.ParentCommentID == null || await GetDb().Comments.AnyAsync(c =>
                    c.CommentID == commentView.ParentCommentID)))
            {
                Comment comment = new Comment();
                if (!await GetDb().Posts.AnyAsync(p => p.PostID == commentView.PostID))
                    return new EmptyResult();
                comment.PostID = commentView.PostID;
                if (await GetDb().Comments.AnyAsync(c => c.CommentID == commentView.ParentCommentID))
                    comment.ParentCommentID = commentView.ParentCommentID;
                comment.AuthorID = User.Identity.GetUserId();
                comment.CreatedOn = new DateTimeOffset(DateTime.Now);
                comment = GetDb().Comments.Add(comment);
                await GetDb().SaveChangesAsync();

                CommentContent commentContent = new CommentContent();
                commentContent.CommentID = comment.CommentID;
                commentContent.Content = commentView.Content;
                GetDb().CommentContents.Add(commentContent);
                await GetDb().SaveChangesAsync();
                return PartialView("Comments/_CommentPartial", comment);
            }
            return new EmptyResult();
        }

        // GET: Blog/Comments/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await GetDb().Comments.FindAsync(id);
            if (comment == null)
            {
                return new EmptyResult();
            }
            EditedCommentView commentView = new EditedCommentView(comment);
            this.AddTemp(commentView, "CommentID", comment.CommentID);
            return PartialView("Comments/_CommentEditPartial", commentView);
        }

        // POST: Blog/Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin,Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "TempTokens,Content,UpdateReason,IsDeleted")] EditedCommentView commentView)
        {
            Comment comment = await GetDb().Comments.FindAsync(this.GetTemp<int>(commentView, "CommentID"));
            if (comment == null)
            {
                this.ClearTemp(commentView);
                return new EmptyResult();
            }
            if (ModelState.IsValid)
            {
                CommentContent commentContent = new CommentContent();
                commentContent.CommentID = comment.CommentID;
                commentContent.EditorID = User.Identity.GetUserId();
                commentContent.UpdatedOn = new DateTimeOffset(DateTime.Now);
                commentContent.UpdateReason = commentView.UpdateReason;
                commentContent.IsDeleted = commentView.IsDeleted;
                commentContent.Content = commentView.Content;
                GetDb().CommentContents.Add(commentContent);
                this.ClearTemp(commentView);
                await GetDb().SaveChangesAsync();
            }
            return PartialView("Comments/_CommentPartial", comment);
            /*
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
            postView.Title = post.Title;
             */
            //return View();
        }
        // GET: Blog/Comments/Delete/5
        [Authorize(Roles = "Admin,Moderator")]
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

        // POST: Blog/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> DeleteConfirmed([Bind(Include = "TempTokens,UpdateReason")] EditedCommentView commentView)
        {
            /*
            Post post = await this.GetDb().Posts.FindAsync(this.GetTemp<int>(postView, "PostID"));
            if (post.Content.IsDeleted) // It was previously deleted
                return RedirectToAction("Index");
            if (post.CreatedOn == null) // It's a draft
            {
                this.GetDb().PostContents.Remove(post.Content);
                this.GetDb().Posts.Remove(post);
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
                this.GetDb().PostContents.Add(postContent);
            }
            await this.GetDb().SaveChangesAsync();
             */
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
