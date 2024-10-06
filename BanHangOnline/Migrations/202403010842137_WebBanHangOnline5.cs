namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tb_New", "CategoryID", "dbo.tb_Category");
            DropForeignKey("dbo.tb_Posts", "CategoryID", "dbo.tb_Category");
            DropIndex("dbo.tb_New", new[] { "CategoryID" });
            DropIndex("dbo.tb_Posts", new[] { "CategoryID" });
            RenameColumn(table: "dbo.tb_New", name: "CategoryID", newName: "Category_Id");
            RenameColumn(table: "dbo.tb_Posts", name: "CategoryID", newName: "Category_Id");
            AlterColumn("dbo.tb_New", "Category_Id", c => c.Int());
            AlterColumn("dbo.tb_Posts", "Category_Id", c => c.Int());
            CreateIndex("dbo.tb_New", "Category_Id");
            CreateIndex("dbo.tb_Posts", "Category_Id");
            AddForeignKey("dbo.tb_New", "Category_Id", "dbo.tb_Category", "Id");
            AddForeignKey("dbo.tb_Posts", "Category_Id", "dbo.tb_Category", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Posts", "Category_Id", "dbo.tb_Category");
            DropForeignKey("dbo.tb_New", "Category_Id", "dbo.tb_Category");
            DropIndex("dbo.tb_Posts", new[] { "Category_Id" });
            DropIndex("dbo.tb_New", new[] { "Category_Id" });
            AlterColumn("dbo.tb_Posts", "Category_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.tb_New", "Category_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.tb_Posts", name: "Category_Id", newName: "CategoryID");
            RenameColumn(table: "dbo.tb_New", name: "Category_Id", newName: "CategoryID");
            CreateIndex("dbo.tb_Posts", "CategoryID");
            CreateIndex("dbo.tb_New", "CategoryID");
            AddForeignKey("dbo.tb_Posts", "CategoryID", "dbo.tb_Category", "Id", cascadeDelete: true);
            AddForeignKey("dbo.tb_New", "CategoryID", "dbo.tb_Category", "Id", cascadeDelete: true);
        }
    }
}
