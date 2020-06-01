namespace WMS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddmodelNoColumn1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "modelNo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "modelNo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
