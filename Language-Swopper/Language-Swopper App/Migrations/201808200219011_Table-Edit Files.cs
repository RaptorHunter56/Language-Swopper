namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableEditFiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.B", "B3", c => c.Int(nullable: false));
            CreateIndex("dbo.B", "B3");
            AddForeignKey("dbo.B", "B3", "dbo.A", "A1", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.B", "B3", "dbo.A");
            DropIndex("dbo.B", new[] { "B3" });
            DropColumn("dbo.B", "B3");
        }
    }
}
