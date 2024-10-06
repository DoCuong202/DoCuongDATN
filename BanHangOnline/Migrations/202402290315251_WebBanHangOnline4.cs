namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_OrderTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        CustomerName = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        TimeLine = c.Int(nullable: false),
                        CountCustomer = c.Int(nullable: false),
                        Employee = c.String(nullable: false),
                        ProductName = c.String(),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        TypePayment = c.String(),
                        statusOrder = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tb_OrderDetail", "OrderTable_Id", c => c.Int());
            CreateIndex("dbo.tb_OrderDetail", "OrderTable_Id");
            AddForeignKey("dbo.tb_OrderDetail", "OrderTable_Id", "dbo.tb_OrderTable", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_OrderDetail", "OrderTable_Id", "dbo.tb_OrderTable");
            DropIndex("dbo.tb_OrderDetail", new[] { "OrderTable_Id" });
            DropColumn("dbo.tb_OrderDetail", "OrderTable_Id");
            DropTable("dbo.tb_OrderTable");
        }
    }
}
