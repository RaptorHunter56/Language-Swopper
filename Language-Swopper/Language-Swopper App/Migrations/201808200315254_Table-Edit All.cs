namespace Language_Swopper_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableEditAll : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.D", "D3", "dbo.C");
            DropForeignKey("dbo.C", "C3", "dbo.B");
            DropForeignKey("dbo.B", "B3", "dbo.A");
            DropForeignKey("dbo.E", "FolderId", "dbo.A");
            DropIndex("dbo.D", new[] { "D3" });
            DropIndex("dbo.C", new[] { "C3" });
            DropIndex("dbo.B", new[] { "B3" });
            DropIndex("dbo.E", new[] { "FolderId" });
            RenameColumn(table: "dbo.E", name: "FolderId", newName: "E4");
            AlterColumn("dbo.D", "D3", c => c.Int());
            AlterColumn("dbo.C", "C3", c => c.Int());
            AlterColumn("dbo.B", "B3", c => c.Int());
            AlterColumn("dbo.E", "E4", c => c.Int());
            CreateIndex("dbo.D", "D3");
            CreateIndex("dbo.C", "C3");
            CreateIndex("dbo.B", "B3");
            CreateIndex("dbo.E", "E4");
            AddForeignKey("dbo.D", "D3", "dbo.C", "C1");
            AddForeignKey("dbo.C", "C3", "dbo.B", "B1");
            AddForeignKey("dbo.B", "B3", "dbo.A", "A1");
            AddForeignKey("dbo.E", "E4", "dbo.A", "A1");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.E", "E4", "dbo.A");
            DropForeignKey("dbo.B", "B3", "dbo.A");
            DropForeignKey("dbo.C", "C3", "dbo.B");
            DropForeignKey("dbo.D", "D3", "dbo.C");
            DropIndex("dbo.E", new[] { "E4" });
            DropIndex("dbo.B", new[] { "B3" });
            DropIndex("dbo.C", new[] { "C3" });
            DropIndex("dbo.D", new[] { "D3" });
            AlterColumn("dbo.E", "E4", c => c.Int(nullable: false));
            AlterColumn("dbo.B", "B3", c => c.Int(nullable: false));
            AlterColumn("dbo.C", "C3", c => c.Int(nullable: false));
            AlterColumn("dbo.D", "D3", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.E", name: "E4", newName: "FolderId");
            CreateIndex("dbo.E", "FolderId");
            CreateIndex("dbo.B", "B3");
            CreateIndex("dbo.C", "C3");
            CreateIndex("dbo.D", "D3");
            AddForeignKey("dbo.E", "FolderId", "dbo.A", "A1", cascadeDelete: true);
            AddForeignKey("dbo.B", "B3", "dbo.A", "A1", cascadeDelete: true);
            AddForeignKey("dbo.C", "C3", "dbo.B", "B1", cascadeDelete: true);
            AddForeignKey("dbo.D", "D3", "dbo.C", "C1", cascadeDelete: true);
        }
    }
}
