namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableEditHighlight2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.E", "E5", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.E", "E5");
        }
    }
}
