using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Areas.Blog.Models
{
    public class PostView
    {
        public string Title { get; set; }
        public string Extract { get; set; }
        public string Content { get; set; }
        public bool DoPublish { get; set; }
    }
    public class EditedPostView : PostView
    {
        public string UpdateReason { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class CommentView
    {
        public Post Post { get; set; }
        public Comment ParentComment { get; set; }
        public string Content { get; set; }
    }
    public class EditedCommentView : CommentView
    {
        public string UpdateReason { get; set; }
        public bool IsDeleted { get; set; }
    }
}