namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableCreateHighlights : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.E",
                c => new
                    {
                        E1 = c.Int(nullable: false, identity: true),
                        E2 = c.String(),
                        E3 = c.String(),
                    })
                .PrimaryKey(t => t.E1);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.E");
        }
    }
}
