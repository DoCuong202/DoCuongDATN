namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebBanHangOnline9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_Rates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCustomer = c.String(),
                        IdProduct = c.String(),
                        startRate = c.String(),
                        content = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tb_Rates");
        }
    }
}
