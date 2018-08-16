namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableCreateFiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.B",
                c => new
                    {
                        B1 = c.Int(nullable: false, identity: true),
                        B2 = c.String(),
                    })
                .PrimaryKey(t => t.B1);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.B");
        }
    }
}
