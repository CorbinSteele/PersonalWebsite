using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Areas.Blog.Models
{
    public interface ITempable
    {
        [Required]
        Dictionary<string, string> TempTokens { get; set; }
    }

    public class PostView
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        [Display(Name = "Extract")]
        public string Extract { get; set; }
        [Required]
        [Display(Name = "Body")]
        public string Content { get; set; }
        [Display(Name = "Publish Post?")]
        public bool DoPublish { get; set; }
    }
    public class EditedPostView : PostView, ITempable
    {
        public EditedPostView()
        {
            TempTokens = new Dictionary<string, string>();
        }
        public Dictionary<string, string> TempTokens { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        [Display(Name = "Edit Reason")]
        public string UpdateReason { get; set; }
        [Display(Name = "Delete Post?")]
        public bool IsDeleted { get; set; }
    }
    public class CommentView
    {
        [Required]
        public Post Post { get; set; }
        public Comment ParentComment { get; set; }
        [Required]
        [Display(Name = "Comment")]
        public string Content { get; set; }
    }
    public class EditedCommentView : CommentView, ITempable
    {
        public EditedCommentView()
        {
            TempTokens = new Dictionary<string, string>();
        }
        public Dictionary<string, string> TempTokens { get; set; }
        [Display(Name = "Edit Reason")]
        public string UpdateReason { get; set; }
        [Display(Name = "Delete Comment?")]
        public bool IsDeleted { get; set; }
    }
}