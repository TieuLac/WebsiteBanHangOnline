namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAll : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tb_OrderDetail", "Product_Id", "dbo.tb_Product");
            DropForeignKey("dbo.tb_OrderDetail", "ProductInventoryId", "dbo.ProductInventories");
            DropIndex("dbo.tb_OrderDetail", new[] { "Product_Id" });
            AddForeignKey("dbo.tb_OrderDetail", "ProductInventoryId", "dbo.ProductInventories", "Id");
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_OrderDetail", "Product_Id", c => c.Int());
            DropForeignKey("dbo.tb_OrderDetail", "ProductInventoryId", "dbo.ProductInventories");
            CreateIndex("dbo.tb_OrderDetail", "Product_Id");
            AddForeignKey("dbo.tb_OrderDetail", "ProductInventoryId", "dbo.ProductInventories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.tb_OrderDetail", "Product_Id", "dbo.tb_Product", "Id");
        }
    }
}
