namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodavanjeModela_Ocena_TipOcena_TipOpisneOcene : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ocena",
                c => new
                    {
                        OcenaId = c.Int(nullable: false, identity: true),
                        Oznaka = c.Int(nullable: false),
                        Plus = c.Boolean(nullable: false),
                        UcenikId = c.Int(nullable: false),
                        CasId = c.Int(nullable: false),
                        TipOceneId = c.Int(nullable: false),
                        TipOpisneOceneId = c.Int(nullable: false),
                        Napomena = c.String(),
                    })
                .PrimaryKey(t => t.OcenaId)
                .ForeignKey("dbo.Cas", t => t.CasId, cascadeDelete: true)
                .ForeignKey("dbo.TipOcene", t => t.TipOceneId, cascadeDelete: true)
                .ForeignKey("dbo.TipOpisneOcene", t => t.TipOpisneOceneId, cascadeDelete: true)
                .ForeignKey("dbo.Ucenik", t => t.UcenikId, cascadeDelete: false)
                .Index(t => t.UcenikId)
                .Index(t => t.CasId)
                .Index(t => t.TipOceneId)
                .Index(t => t.TipOpisneOceneId);
            
            CreateTable(
                "dbo.TipOcene",
                c => new
                    {
                        TipOceneId = c.Int(nullable: false, identity: true),
                        Tip = c.String(),
                    })
                .PrimaryKey(t => t.TipOceneId);
            
            CreateTable(
                "dbo.TipOpisneOcene",
                c => new
                    {
                        TipOpisneOceneId = c.Int(nullable: false, identity: true),
                        Tip = c.String(),
                    })
                .PrimaryKey(t => t.TipOpisneOceneId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ocena", "UcenikId", "dbo.Ucenik");
            DropForeignKey("dbo.Ocena", "TipOpisneOceneId", "dbo.TipOpisneOcene");
            DropForeignKey("dbo.Ocena", "TipOceneId", "dbo.TipOcene");
            DropForeignKey("dbo.Ocena", "CasId", "dbo.Cas");
            DropIndex("dbo.Ocena", new[] { "TipOpisneOceneId" });
            DropIndex("dbo.Ocena", new[] { "TipOceneId" });
            DropIndex("dbo.Ocena", new[] { "CasId" });
            DropIndex("dbo.Ocena", new[] { "UcenikId" });
            DropTable("dbo.TipOpisneOcene");
            DropTable("dbo.TipOcene");
            DropTable("dbo.Ocena");
        }
    }
}
