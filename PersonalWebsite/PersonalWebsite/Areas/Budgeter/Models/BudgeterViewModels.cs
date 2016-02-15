using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Areas.Budgeter.Models
{
    /*public class PostView
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        [Display(Name = "Extract")]
        public string Extract { get; set; }
        private string content;
        [Required]
        [Display(Name = "Body")]
        [System.Web.Mvc.AllowHtml]
        public string Content
        {
            get { return content; }
            set { content = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(value).Trim(); }
        }
        [Display(Name = "Publish Post?")]
        public bool DoPublish { get; set; }
    }
    public class EditedPostView : PostView, ITempable
    {
        public EditedPostView()
        {
            TempTokens = new Dictionary<string, string>();
        }
        public EditedPostView(Post post) : this()
        {
            this.DoPublish = post.CreatedOn != null;
            this.Title = post.Title;
            this.Extract = post.Content.Extract;
            this.Content = post.Content.Content;
            this.IsDeleted = post.Content.IsDeleted;
            this.UpdateReason = post.Content.UpdateReason;
        }
        public Dictionary<string, string> TempTokens { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        [Display(Name = "Reason")]
        public string UpdateReason { get; set; }
        [Display(Name = "Delete Post?")]
        public bool IsDeleted { get; set; }
    }
    public class CommentView
    {
        public CommentView() { }
        public CommentView(int postId, int? parentCommentID)
        {
            this.PostID = postId;
            this.ParentCommentID = parentCommentID;
        }
        [Required]
        public int PostID { get; set; }
        public int? ParentCommentID { get; set; }
        private string content;
        [Required]
        [Display(Name = "Comment")]
        [System.Web.Mvc.AllowHtml]
        public string Content
        {
            get { return content; }
            set { content = Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(value).Trim(); }
        }
    }
    public class EditedCommentView : CommentView, ITempable
    {
        public EditedCommentView()
        {
            TempTokens = new Dictionary<string, string>();
        }
        public EditedCommentView(Comment comment) : this()
        {
            this.PostID = comment.PostID;
            this.ParentCommentID = comment.ParentCommentID;
            this.Content = comment.Content.Content;
            this.UpdateReason = comment.Content.UpdateReason;
            this.IsDeleted = comment.Content.IsDeleted;
        }
        public Dictionary<string, string> TempTokens { get; set; }
        [Display(Name = "Edit Reason")]
        public string UpdateReason { get; set; }
        [Display(Name = "Delete Comment?")]
        public bool IsDeleted { get; set; }
    }*/
}