namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LicencaIZvanjeRequiredAnotacija : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Profesor", "Zvanje", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Profesor", "Zvanje", c => c.String());
        }
    }
}
