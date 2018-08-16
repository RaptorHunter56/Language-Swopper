namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableCreateFolders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.A",
                c => new
                    {
                        A1 = c.Int(nullable: false, identity: true),
                        A2 = c.String(),
                        A3 = c.String(),
                    })
                .PrimaryKey(t => t.A1);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.A");
        }
    }
}
