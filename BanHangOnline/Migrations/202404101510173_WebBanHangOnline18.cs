namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_OrderTable", "CreatedBy", c => c.String());
            AddColumn("dbo.tb_OrderTable", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_OrderTable", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_OrderTable", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_OrderTable", "ModifiedBy");
            DropColumn("dbo.tb_OrderTable", "ModifiedDate");
            DropColumn("dbo.tb_OrderTable", "CreatedDate");
            DropColumn("dbo.tb_OrderTable", "CreatedBy");
        }
    }
}
