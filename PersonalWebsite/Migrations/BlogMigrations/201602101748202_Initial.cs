namespace PersonalWebsite.Migrations.BlogMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentContents",
                c => new
                    {
                        CommentContentID = c.Int(nullable: false, identity: true),
                        CommentID = c.Int(nullable: false),
                        Content = c.String(),
                        EditorID = c.String(),
                        UpdatedOn = c.DateTimeOffset(precision: 7),
                        UpdateReason = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CommentContentID)
                .ForeignKey("dbo.Comments", t => t.CommentID, cascadeDelete: true)
                .Index(t => t.CommentID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(nullable: false),
                        ParentCommentID = c.Int(),
                        AuthorID = c.String(),
                        CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Comments", t => t.ParentCommentID)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID)
                .Index(t => t.ParentCommentID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        AuthorID = c.String(),
                        Title = c.String(),
                        CreatedOn = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.PostID);
            
            CreateTable(
                "dbo.PostContents",
                c => new
                    {
                        PostContentID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(nullable: false),
                        Content = c.String(),
                        Extract = c.String(),
                        EditorID = c.String(),
                        UpdatedOn = c.DateTimeOffset(precision: 7),
                        UpdateReason = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostContentID)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostContents", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Comments", "ParentCommentID", "dbo.Comments");
            DropForeignKey("dbo.CommentContents", "CommentID", "dbo.Comments");
            DropIndex("dbo.PostContents", new[] { "PostID" });
            DropIndex("dbo.Comments", new[] { "ParentCommentID" });
            DropIndex("dbo.Comments", new[] { "PostID" });
            DropIndex("dbo.CommentContents", new[] { "CommentID" });
            DropTable("dbo.PostContents");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
            DropTable("dbo.CommentContents");
        }
    }
}
