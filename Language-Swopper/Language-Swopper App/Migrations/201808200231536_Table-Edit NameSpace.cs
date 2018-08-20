namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableEditNameSpace : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.D", "D3", c => c.Int(nullable: false));
            CreateIndex("dbo.D", "D3");
            AddForeignKey("dbo.D", "D3", "dbo.C", "C1", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.D", "D3", "dbo.C");
            DropIndex("dbo.D", new[] { "D3" });
            DropColumn("dbo.D", "D3");
        }
    }
}
