namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpajanjeSkolskeGodineSaOdeljenjem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Odeljenje", "SkolskaGodinaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Odeljenje", "SkolskaGodinaId");
            AddForeignKey("dbo.Odeljenje", "SkolskaGodinaId", "dbo.SkolskaGodina", "SkolskaGodinaId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Odeljenje", "SkolskaGodinaId", "dbo.SkolskaGodina");
            DropIndex("dbo.Odeljenje", new[] { "SkolskaGodinaId" });
            DropColumn("dbo.Odeljenje", "SkolskaGodinaId");
        }
    }
}
