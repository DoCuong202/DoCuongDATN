namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Rates", "NameUser", c => c.String());
            DropColumn("dbo.tb_Rates", "IdCustomer");
            DropColumn("dbo.tb_Rates", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Rates", "UserName", c => c.String());
            AddColumn("dbo.tb_Rates", "IdCustomer", c => c.String());
            DropColumn("dbo.tb_Rates", "NameUser");
        }
    }
}
