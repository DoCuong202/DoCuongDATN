namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline20 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_OrderTableDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        OrderTable_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_OrderTable", t => t.OrderTable_Id)
                .ForeignKey("dbo.tb_Product", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.OrderTable_Id);
            
            AddColumn("dbo.tb_OrderTable", "Day", c => c.String());
            AlterColumn("dbo.tb_OrderTable", "CountCustomer", c => c.String());
            DropColumn("dbo.tb_OrderTable", "TableID");
            DropColumn("dbo.tb_OrderTable", "Employee");
            DropColumn("dbo.tb_OrderTable", "ProductID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_OrderTable", "ProductID", c => c.String());
            AddColumn("dbo.tb_OrderTable", "Employee", c => c.String());
            AddColumn("dbo.tb_OrderTable", "TableID", c => c.String());
            DropForeignKey("dbo.tb_OrderTableDetail", "ProductID", "dbo.tb_Product");
            DropForeignKey("dbo.tb_OrderTableDetail", "OrderTable_Id", "dbo.tb_OrderTable");
            DropIndex("dbo.tb_OrderTableDetail", new[] { "OrderTable_Id" });
            DropIndex("dbo.tb_OrderTableDetail", new[] { "ProductID" });
            AlterColumn("dbo.tb_OrderTable", "CountCustomer", c => c.Int(nullable: false));
            DropColumn("dbo.tb_OrderTable", "Day");
            DropTable("dbo.tb_OrderTableDetail");
        }
    }
}
