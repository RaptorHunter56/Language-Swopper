namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableCreateNameSpaces : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.C",
                c => new
                    {
                        C1 = c.Int(nullable: false, identity: true),
                        C2 = c.String(),
                    })
                .PrimaryKey(t => t.C1);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.C");
        }
    }
}
