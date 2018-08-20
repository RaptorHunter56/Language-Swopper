namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableEditHighlight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.E", "FolderId", c => c.Int(nullable: false));
            CreateIndex("dbo.E", "FolderId");
            AddForeignKey("dbo.E", "FolderId", "dbo.A", "A1", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.E", "FolderId", "dbo.A");
            DropIndex("dbo.E", new[] { "FolderId" });
            DropColumn("dbo.E", "FolderId");
        }
    }
}
