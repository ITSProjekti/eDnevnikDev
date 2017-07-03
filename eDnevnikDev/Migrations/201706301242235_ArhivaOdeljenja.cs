namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArhivaOdeljenja : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SmerOdeljenje", "Smer_SmerID", "dbo.Smer");
            DropForeignKey("dbo.SmerOdeljenje", "Odeljenje_Id", "dbo.Odeljenje");
            DropIndex("dbo.SmerOdeljenje", new[] { "Smer_SmerID" });
            DropIndex("dbo.SmerOdeljenje", new[] { "Odeljenje_Id" });
            CreateTable(
                "dbo.ArhivaOdeljenja",
                c => new
                    {
                        ArhivaOdeljenjaID = c.Int(nullable: false, identity: true),
                        OdeljenjeID = c.Int(nullable: false),
                        UcenikID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArhivaOdeljenjaID)
                .ForeignKey("dbo.Odeljenje", t => t.OdeljenjeID, cascadeDelete: false)
                .ForeignKey("dbo.Ucenik", t => t.UcenikID, cascadeDelete: false)
                .Index(t => t.OdeljenjeID)
                .Index(t => t.UcenikID);
            
            CreateTable(
                "dbo.Oznaka",
                c => new
                    {
                        OznakaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OznakaId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Opis = c.String(),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "dbo.SmerOznaka",
                c => new
                    {
                        Smer_SmerID = c.Int(nullable: false),
                        Oznaka_OznakaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Smer_SmerID, t.Oznaka_OznakaId })
                .ForeignKey("dbo.Smer", t => t.Smer_SmerID, cascadeDelete: true)
                .ForeignKey("dbo.Oznaka", t => t.Oznaka_OznakaId, cascadeDelete: true)
                .Index(t => t.Smer_SmerID)
                .Index(t => t.Oznaka_OznakaId);
            
            AddColumn("dbo.Odeljenje", "OznakaID", c => c.Int(nullable: false));
            AddColumn("dbo.Odeljenje", "StatusID", c => c.Int(nullable: false));
            AddColumn("dbo.Odeljenje", "SkolskaGodina", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Odeljenje", "OznakaID");
            CreateIndex("dbo.Odeljenje", "StatusID");
            AddForeignKey("dbo.Odeljenje", "OznakaID", "dbo.Oznaka", "OznakaId", cascadeDelete: true);
            AddForeignKey("dbo.Odeljenje", "StatusID", "dbo.Status", "StatusId", cascadeDelete: true);
            DropColumn("dbo.Odeljenje", "Oznaka");
            DropTable("dbo.SmerOdeljenje");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SmerOdeljenje",
                c => new
                    {
                        Smer_SmerID = c.Int(nullable: false),
                        Odeljenje_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Smer_SmerID, t.Odeljenje_Id });
            
            AddColumn("dbo.Odeljenje", "Oznaka", c => c.Int(nullable: false));
            DropForeignKey("dbo.ArhivaOdeljenja", "UcenikID", "dbo.Ucenik");
            DropForeignKey("dbo.ArhivaOdeljenja", "OdeljenjeID", "dbo.Odeljenje");
            DropForeignKey("dbo.Odeljenje", "StatusID", "dbo.Status");
            DropForeignKey("dbo.Odeljenje", "OznakaID", "dbo.Oznaka");
            DropForeignKey("dbo.SmerOznaka", "Oznaka_OznakaId", "dbo.Oznaka");
            DropForeignKey("dbo.SmerOznaka", "Smer_SmerID", "dbo.Smer");
            DropIndex("dbo.SmerOznaka", new[] { "Oznaka_OznakaId" });
            DropIndex("dbo.SmerOznaka", new[] { "Smer_SmerID" });
            DropIndex("dbo.Odeljenje", new[] { "StatusID" });
            DropIndex("dbo.Odeljenje", new[] { "OznakaID" });
            DropIndex("dbo.ArhivaOdeljenja", new[] { "UcenikID" });
            DropIndex("dbo.ArhivaOdeljenja", new[] { "OdeljenjeID" });
            DropColumn("dbo.Odeljenje", "SkolskaGodina");
            DropColumn("dbo.Odeljenje", "StatusID");
            DropColumn("dbo.Odeljenje", "OznakaID");
            DropTable("dbo.SmerOznaka");
            DropTable("dbo.Status");
            DropTable("dbo.Oznaka");
            DropTable("dbo.ArhivaOdeljenja");
            CreateIndex("dbo.SmerOdeljenje", "Odeljenje_Id");
            CreateIndex("dbo.SmerOdeljenje", "Smer_SmerID");
            AddForeignKey("dbo.SmerOdeljenje", "Odeljenje_Id", "dbo.Odeljenje", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SmerOdeljenje", "Smer_SmerID", "dbo.Smer", "SmerID", cascadeDelete: true);
        }
    }
}
