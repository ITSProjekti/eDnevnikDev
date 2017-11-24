namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aktuelnaSkolskaGodina : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SkolskaGodina", "Aktuelna", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SkolskaGodina", "Aktuelna");
        }
    }
}
