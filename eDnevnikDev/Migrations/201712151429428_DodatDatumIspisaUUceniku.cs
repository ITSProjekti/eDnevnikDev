namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatDatumIspisaUUceniku : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "DatumIspisa", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "DatumIspisa");
        }
    }
}
