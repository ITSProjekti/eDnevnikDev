namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class proba : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SmerOznaka", newName: "OznakaSmer");
            DropPrimaryKey("dbo.OznakaSmer");
            CreateTable(
                "dbo.Cas",
                c => new
                    {
                        CasId = c.Int(nullable: false, identity: true),
                        Datum = c.DateTime(nullable: false),
                        Opis = c.String(),
                        ProfesorId = c.Int(nullable: false),
                        PredmetId = c.Int(nullable: false),
                        OdeljenjeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CasId)
                .ForeignKey("dbo.Odeljenje", t => t.OdeljenjeId, cascadeDelete: true)
                .ForeignKey("dbo.Predmet", t => t.PredmetId, cascadeDelete: true)
                .ForeignKey("dbo.Profesor", t => t.ProfesorId, cascadeDelete: true)
                .Index(t => t.ProfesorId)
                .Index(t => t.PredmetId)
                .Index(t => t.OdeljenjeId);
            
            CreateTable(
                "dbo.Napomena",
                c => new
                    {
                        NapomenaId = c.Int(nullable: false, identity: true),
                        Opis = c.String(),
                        CasId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NapomenaId)
                .ForeignKey("dbo.Cas", t => t.CasId, cascadeDelete: true)
                .Index(t => t.CasId);
            
            CreateTable(
                "dbo.Odsustvo",
                c => new
                    {
                        OdsustvoId = c.Int(nullable: false, identity: true),
                        UcenikId = c.Int(nullable: false),
                        CasId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OdsustvoId)
                .ForeignKey("dbo.Cas", t => t.CasId, cascadeDelete: true)
                .ForeignKey("dbo.Ucenik", t => t.UcenikId, cascadeDelete: false)
                .Index(t => t.UcenikId)
                .Index(t => t.CasId);
            
            AddPrimaryKey("dbo.OznakaSmer", new[] { "Oznaka_OznakaId", "Smer_SmerID" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cas", "ProfesorId", "dbo.Profesor");
            DropForeignKey("dbo.Cas", "PredmetId", "dbo.Predmet");
            DropForeignKey("dbo.Odsustvo", "UcenikId", "dbo.Ucenik");
            DropForeignKey("dbo.Odsustvo", "CasId", "dbo.Cas");
            DropForeignKey("dbo.Cas", "OdeljenjeId", "dbo.Odeljenje");
            DropForeignKey("dbo.Napomena", "CasId", "dbo.Cas");
            DropIndex("dbo.Odsustvo", new[] { "CasId" });
            DropIndex("dbo.Odsustvo", new[] { "UcenikId" });
            DropIndex("dbo.Napomena", new[] { "CasId" });
            DropIndex("dbo.Cas", new[] { "OdeljenjeId" });
            DropIndex("dbo.Cas", new[] { "PredmetId" });
            DropIndex("dbo.Cas", new[] { "ProfesorId" });
            DropPrimaryKey("dbo.OznakaSmer");
            DropTable("dbo.Odsustvo");
            DropTable("dbo.Napomena");
            DropTable("dbo.Cas");
            AddPrimaryKey("dbo.OznakaSmer", new[] { "Smer_SmerID", "Oznaka_OznakaId" });
            RenameTable(name: "dbo.OznakaSmer", newName: "SmerOznaka");
        }
    }
}
