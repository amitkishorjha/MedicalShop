namespace WMS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddmodelNoColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "modelNo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Products", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Price");
            DropColumn("dbo.Products", "modelNo");
        }
    }
}
