namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "statusOrder", c => c.Int(nullable: false));
            DropColumn("dbo.tb_Order", "status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Order", "status", c => c.String());
            DropColumn("dbo.tb_Order", "statusOrder");
        }
    }
}
