namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PredmetiOdeljenja : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PredmetOdeljenje",
                c => new
                    {
                        Predmet_PredmetID = c.Int(nullable: false),
                        Odeljenje_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Predmet_PredmetID, t.Odeljenje_Id })
                .ForeignKey("dbo.Predmet", t => t.Predmet_PredmetID, cascadeDelete: true)
                .ForeignKey("dbo.Odeljenje", t => t.Odeljenje_Id, cascadeDelete: true)
                .Index(t => t.Predmet_PredmetID)
                .Index(t => t.Odeljenje_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PredmetOdeljenje", "Odeljenje_Id", "dbo.Odeljenje");
            DropForeignKey("dbo.PredmetOdeljenje", "Predmet_PredmetID", "dbo.Predmet");
            DropIndex("dbo.PredmetOdeljenje", new[] { "Odeljenje_Id" });
            DropIndex("dbo.PredmetOdeljenje", new[] { "Predmet_PredmetID" });
            DropTable("dbo.PredmetOdeljenje");
        }
    }
}
