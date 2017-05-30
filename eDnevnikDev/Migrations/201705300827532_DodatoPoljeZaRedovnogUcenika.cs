namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatoPoljeZaRedovnogUcenika : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "Vanredan", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "Vanredan");
        }
    }
}
