namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableCreateClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.D",
                c => new
                    {
                        D1 = c.Int(nullable: false, identity: true),
                        D2 = c.String(),
                    })
                .PrimaryKey(t => t.D1);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.D");
        }
    }
}
