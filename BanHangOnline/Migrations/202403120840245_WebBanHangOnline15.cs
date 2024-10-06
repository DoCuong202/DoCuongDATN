namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Rates", "ContentRate", c => c.String());
            AddColumn("dbo.tb_Rates", "NgayTao", c => c.DateTime(nullable: false));
            DropColumn("dbo.tb_Rates", "content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Rates", "content", c => c.String());
            DropColumn("dbo.tb_Rates", "NgayTao");
            DropColumn("dbo.tb_Rates", "ContentRate");
        }
    }
}
