namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UbacenDatumUnosaUUcenika : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "DatumUnosa", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "DatumUnosa");
        }
    }
}
