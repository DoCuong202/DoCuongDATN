namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_OrderTable", "TableID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_OrderTable", "TableID");
        }
    }
}
