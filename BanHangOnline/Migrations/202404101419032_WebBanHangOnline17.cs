namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_OrderTable", "ProductID", c => c.String());
            AddColumn("dbo.tb_OrderTable", "statusPayment", c => c.Int(nullable: false));
            AlterColumn("dbo.tb_OrderTable", "TimeLine", c => c.String(nullable: false));
            AlterColumn("dbo.tb_OrderTable", "Employee", c => c.String());
            DropColumn("dbo.tb_OrderTable", "ProductName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_OrderTable", "ProductName", c => c.String());
            AlterColumn("dbo.tb_OrderTable", "Employee", c => c.String(nullable: false));
            AlterColumn("dbo.tb_OrderTable", "TimeLine", c => c.Int(nullable: false));
            DropColumn("dbo.tb_OrderTable", "statusPayment");
            DropColumn("dbo.tb_OrderTable", "ProductID");
        }
    }
}
