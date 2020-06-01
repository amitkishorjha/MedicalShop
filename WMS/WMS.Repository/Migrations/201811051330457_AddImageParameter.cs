namespace WMS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageParameter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ImagePath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ImagePath");
        }
    }
}
