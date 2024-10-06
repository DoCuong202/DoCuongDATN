namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Rates", "UserName", c => c.String());
            AlterColumn("dbo.tb_Rates", "startRate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Rates", "startRate", c => c.String());
            DropColumn("dbo.tb_Rates", "UserName");
        }
    }
}
