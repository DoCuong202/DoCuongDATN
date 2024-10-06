namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline25 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_OrderTable", "TimeStart", c => c.String());
            AlterColumn("dbo.tb_OrderTable", "TimeEnd", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_OrderTable", "TimeEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tb_OrderTable", "TimeStart", c => c.DateTime(nullable: false));
        }
    }
}
