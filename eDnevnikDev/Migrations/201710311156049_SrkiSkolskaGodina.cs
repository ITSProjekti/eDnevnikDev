namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SrkiSkolskaGodina : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Polugodiste",
                c => new
                    {
                        PolugodisteId = c.Int(nullable: false, identity: true),
                        SkolskaGodinaId = c.Int(nullable: false),
                        PocetakPolugodista = c.DateTime(nullable: false),
                        KrajPolugodista = c.DateTime(nullable: false),
                        TipPolugodista = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PolugodisteId)
                .ForeignKey("dbo.SkolskaGodina", t => t.SkolskaGodinaId, cascadeDelete: true)
                .Index(t => t.SkolskaGodinaId);
            
            CreateTable(
                "dbo.SkolskaGodina",
                c => new
                    {
                        SkolskaGodinaId = c.Int(nullable: false, identity: true),
                        PocetakSkolskeGodine = c.DateTime(nullable: false),
                        KrajSkolskeGodine = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SkolskaGodinaId);
            
            CreateTable(
                "dbo.Tromesecje",
                c => new
                    {
                        TromesecjeId = c.Int(nullable: false, identity: true),
                        PolugodisteId = c.Int(nullable: false),
                        PocetakTromesecja = c.DateTime(nullable: false),
                        KrajTromesecja = c.DateTime(nullable: false),
                        TipTromesecja = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TromesecjeId)
                .ForeignKey("dbo.Polugodiste", t => t.PolugodisteId, cascadeDelete: true)
                .Index(t => t.PolugodisteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tromesecje", "PolugodisteId", "dbo.Polugodiste");
            DropForeignKey("dbo.Polugodiste", "SkolskaGodinaId", "dbo.SkolskaGodina");
            DropIndex("dbo.Tromesecje", new[] { "PolugodisteId" });
            DropIndex("dbo.Polugodiste", new[] { "SkolskaGodinaId" });
            DropTable("dbo.Tromesecje");
            DropTable("dbo.SkolskaGodina");
            DropTable("dbo.Polugodiste");
        }
    }
}
