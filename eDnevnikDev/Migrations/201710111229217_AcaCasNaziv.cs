namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AcaCasNaziv : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cas", "Naziv", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cas", "Naziv");
        }
    }
}
