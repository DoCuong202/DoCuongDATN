namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_OrderTable", "TimeStart", c => c.String());
            AddColumn("dbo.tb_OrderTable", "TimeEnd", c => c.String());
            DropColumn("dbo.tb_OrderTable", "TimeLine");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_OrderTable", "TimeLine", c => c.String(nullable: false));
            DropColumn("dbo.tb_OrderTable", "TimeEnd");
            DropColumn("dbo.tb_OrderTable", "TimeStart");
        }
    }
}
