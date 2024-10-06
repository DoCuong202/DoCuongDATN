namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline23 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tb_OrderTableDetail", "OrderTable_Id", "dbo.tb_OrderTable");
            DropIndex("dbo.tb_OrderTableDetail", new[] { "OrderTable_Id" });
            RenameColumn(table: "dbo.tb_OrderTableDetail", name: "OrderTable_Id", newName: "OrderTableID");
            AlterColumn("dbo.tb_OrderTableDetail", "OrderTableID", c => c.Int(nullable: false));
            CreateIndex("dbo.tb_OrderTableDetail", "OrderTableID");
            AddForeignKey("dbo.tb_OrderTableDetail", "OrderTableID", "dbo.tb_OrderTable", "Id", cascadeDelete: true);
            DropColumn("dbo.tb_OrderTableDetail", "OrderID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_OrderTableDetail", "OrderID", c => c.Int(nullable: false));
            DropForeignKey("dbo.tb_OrderTableDetail", "OrderTableID", "dbo.tb_OrderTable");
            DropIndex("dbo.tb_OrderTableDetail", new[] { "OrderTableID" });
            AlterColumn("dbo.tb_OrderTableDetail", "OrderTableID", c => c.Int());
            RenameColumn(table: "dbo.tb_OrderTableDetail", name: "OrderTableID", newName: "OrderTable_Id");
            CreateIndex("dbo.tb_OrderTableDetail", "OrderTable_Id");
            AddForeignKey("dbo.tb_OrderTableDetail", "OrderTable_Id", "dbo.tb_OrderTable", "Id");
        }
    }
}
