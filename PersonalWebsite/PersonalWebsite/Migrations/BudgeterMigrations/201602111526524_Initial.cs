namespace PersonalWebsite.Migrations.BudgeterMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "UpdatedByID", c => c.String());
            AddColumn("dbo.Transactions", "UpdatedByID", c => c.String());
            DropColumn("dbo.Categories", "UpdatedBy");
            DropColumn("dbo.Transactions", "UpdatedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "UpdatedBy", c => c.String());
            AddColumn("dbo.Categories", "UpdatedBy", c => c.String());
            DropColumn("dbo.Transactions", "UpdatedByID");
            DropColumn("dbo.Categories", "UpdatedByID");
        }
    }
}
