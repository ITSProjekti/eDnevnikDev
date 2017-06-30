namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatJedinstveniBrojUUcenika : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "JedinstveniBroj", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "JedinstveniBroj");
        }
    }
}
