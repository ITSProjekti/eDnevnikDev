namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IzmenjenoOdeljenjeDodataKolekcijaZaSmer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Odeljenje", "SmerID", "dbo.Smer");
            DropIndex("dbo.Odeljenje", new[] { "SmerID" });
            CreateTable(
                "dbo.OdeljenjeSmer",
                c => new
                    {
                        Odeljenje_Id = c.Int(nullable: false),
                        Smer_SmerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Odeljenje_Id, t.Smer_SmerID })
                .ForeignKey("dbo.Odeljenje", t => t.Odeljenje_Id, cascadeDelete: true)
                .ForeignKey("dbo.Smer", t => t.Smer_SmerID, cascadeDelete: true)
                .Index(t => t.Odeljenje_Id)
                .Index(t => t.Smer_SmerID);
            
            DropColumn("dbo.Odeljenje", "SmerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Odeljenje", "SmerID", c => c.Int(nullable: false));
            DropForeignKey("dbo.OdeljenjeSmer", "Smer_SmerID", "dbo.Smer");
            DropForeignKey("dbo.OdeljenjeSmer", "Odeljenje_Id", "dbo.Odeljenje");
            DropIndex("dbo.OdeljenjeSmer", new[] { "Smer_SmerID" });
            DropIndex("dbo.OdeljenjeSmer", new[] { "Odeljenje_Id" });
            DropTable("dbo.OdeljenjeSmer");
            CreateIndex("dbo.Odeljenje", "SmerID");
            AddForeignKey("dbo.Odeljenje", "SmerID", "dbo.Smer", "SmerID", cascadeDelete: true);
        }
    }
}
