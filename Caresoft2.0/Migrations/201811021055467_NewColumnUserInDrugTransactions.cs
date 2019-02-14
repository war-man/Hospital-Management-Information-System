namespace Caresoft2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewColumnUserInDrugTransactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrugTransactions", "User", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DrugTransactions", "User");
        }
    }
}
