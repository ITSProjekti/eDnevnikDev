namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatDatumRodjenjaUModelUcenika : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "DatumRodjenja", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "DatumRodjenja");
        }
    }
}
