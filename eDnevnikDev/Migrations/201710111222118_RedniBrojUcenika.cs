namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RedniBrojUcenika : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "RedniBroj", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "RedniBroj");
        }
    }
}
