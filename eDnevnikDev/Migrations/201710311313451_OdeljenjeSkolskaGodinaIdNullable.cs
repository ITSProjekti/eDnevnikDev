namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OdeljenjeSkolskaGodinaIdNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Odeljenje", "SkolskaGodinaId", "dbo.SkolskaGodina");
            DropIndex("dbo.Odeljenje", new[] { "SkolskaGodinaId" });
            AlterColumn("dbo.Odeljenje", "SkolskaGodinaId", c => c.Int());
            CreateIndex("dbo.Odeljenje", "SkolskaGodinaId");
            AddForeignKey("dbo.Odeljenje", "SkolskaGodinaId", "dbo.SkolskaGodina", "SkolskaGodinaId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Odeljenje", "SkolskaGodinaId", "dbo.SkolskaGodina");
            DropIndex("dbo.Odeljenje", new[] { "SkolskaGodinaId" });
            AlterColumn("dbo.Odeljenje", "SkolskaGodinaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Odeljenje", "SkolskaGodinaId");
            AddForeignKey("dbo.Odeljenje", "SkolskaGodinaId", "dbo.SkolskaGodina", "SkolskaGodinaId", cascadeDelete: true);
        }
    }
}
