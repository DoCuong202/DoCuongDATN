namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "IdCustomer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "IdCustomer");
        }
    }
}
