﻿using PersonalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Areas.Blog.Models
{
    public class Post
    {
        public Post()
        {
            this.PostContents = new HashSet<PostContent>();
            this.Comments = new HashSet<Comment>();
        }

        public int PostID { get; set; }
        public string AuthorID { get; set; }
        public string Title { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        public ApplicationUser GetAuthor(ApplicationUserManager userManager)
        {
            return userManager.FindUserOrDefault(AuthorID).Result;
        }
        public virtual ICollection<PostContent> PostContents { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public PostContent Content {
            get { return this.PostContents.OrderByDescending(pc => pc.PostContentID).First(); }
        }
    }
    public class PostContent
    {
        public int PostContentID { get; set; }
        public int PostID { get; set; }
        public string Content { get; set; }
        public string Extract { get; set; }
        public string EditorID { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string UpdateReason { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Post Post { get; set; }
        public ApplicationUser GetEditor(ApplicationUserManager userManager)
        {
            return userManager.FindUserOrDefault(EditorID).Result;
        }
    }
    public class Comment
    {
        public Comment()
        {
            this.Comments = new HashSet<Comment>();
            this.CommentContents = new HashSet<CommentContent>();
        }

        public int CommentID { get; set; }
        public int PostID { get; set; }
        public int? ParentCommentID { get; set; }
        public string AuthorID { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public virtual Post Post { get; set; }
        public virtual Comment Parent { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public ApplicationUser GetAuthor(ApplicationUserManager userManager)
        {
            return userManager.FindUserOrDefault(AuthorID).Result;
        }
        public virtual ICollection<CommentContent> CommentContents { get; set; }

        public CommentContent Content
        {
            get { return this.CommentContents.OrderByDescending(cc => cc.CommentContentID).First(); }
        }
    }
    public class CommentContent
    {
        public int CommentContentID { get; set; }
        public int CommentID { get; set; }
        public string Content { get; set; }
        public string EditorID { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string UpdateReason { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Comment Comment { get; set; }
        public ApplicationUser GetEditor(ApplicationUserManager userManager)
        {
            return userManager.FindUserOrDefault(EditorID).Result;
        }
    }
}