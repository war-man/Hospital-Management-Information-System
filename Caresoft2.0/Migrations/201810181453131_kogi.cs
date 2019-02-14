namespace Caresoft2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kogi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockAdjusteds", "Type", c => c.String());
            AddColumn("dbo.StockAdjusteds", "Department", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockAdjusteds", "Department");
            DropColumn("dbo.StockAdjusteds", "Type");
        }
    }
}
