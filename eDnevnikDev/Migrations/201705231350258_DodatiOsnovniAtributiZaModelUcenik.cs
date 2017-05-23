namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatiOsnovniAtributiZaModelUcenik : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ucenik", "Ime", c => c.String(nullable: false));
            AlterColumn("dbo.Ucenik", "Prezime", c => c.String(nullable: false));
            AlterColumn("dbo.Ucenik", "Adresa", c => c.String(nullable: false));
            AlterColumn("dbo.Ucenik", "RoditeljStaratelj", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ucenik", "RoditeljStaratelj", c => c.String());
            AlterColumn("dbo.Ucenik", "Adresa", c => c.String());
            AlterColumn("dbo.Ucenik", "Prezime", c => c.String());
            AlterColumn("dbo.Ucenik", "Ime", c => c.String());
        }
    }
}
