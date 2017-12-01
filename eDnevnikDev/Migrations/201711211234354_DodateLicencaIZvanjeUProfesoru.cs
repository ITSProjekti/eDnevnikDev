namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodateLicencaIZvanjeUProfesoru : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profesor", "Licenca", c => c.Boolean(nullable: false));
            AddColumn("dbo.Profesor", "Zvanje", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profesor", "Zvanje");
            DropColumn("dbo.Profesor", "Licenca");
        }
    }
}
