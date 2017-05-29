namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatRequiredZaJmbg : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ucenik", "JMBG", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ucenik", "JMBG", c => c.String());
        }
    }
}
