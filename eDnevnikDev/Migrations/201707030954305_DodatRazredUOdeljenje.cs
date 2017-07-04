namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatRazredUOdeljenje : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Odeljenje", "Razred", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Odeljenje", "Razred");
        }
    }
}
