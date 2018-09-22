namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableCreateOpenFilePaths : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.F",
                c => new
                    {
                        F1 = c.Int(nullable: false, identity: true),
                        F2 = c.String(),
                        F3 = c.Int(),
                    })
                .PrimaryKey(t => t.F1)
                .ForeignKey("dbo.A", t => t.F3)
                .Index(t => t.F3);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.F", "F3", "dbo.A");
            DropIndex("dbo.F", new[] { "F3" });
            DropTable("dbo.F");
        }
    }
}
