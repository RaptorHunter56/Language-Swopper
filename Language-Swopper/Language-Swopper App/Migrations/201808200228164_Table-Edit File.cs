namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableEditFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.C", "C3", c => c.Int(nullable: false));
            CreateIndex("dbo.C", "C3");
            AddForeignKey("dbo.C", "C3", "dbo.B", "B1", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.C", "C3", "dbo.B");
            DropIndex("dbo.C", new[] { "C3" });
            DropColumn("dbo.C", "C3");
        }
    }
}
