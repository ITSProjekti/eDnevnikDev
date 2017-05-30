namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodataAdresaITelefonZaProfesora : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profesor", "Telefon", c => c.String(nullable: false));
            AddColumn("dbo.Profesor", "Adresa", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profesor", "Adresa");
            DropColumn("dbo.Profesor", "Telefon");
        }
    }
}
