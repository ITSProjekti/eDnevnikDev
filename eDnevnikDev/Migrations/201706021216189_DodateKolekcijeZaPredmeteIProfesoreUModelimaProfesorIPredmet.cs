namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodateKolekcijeZaPredmeteIProfesoreUModelimaProfesorIPredmet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfesorPredmet",
                c => new
                    {
                        Profesor_ProfesorID = c.Int(nullable: false),
                        Predmet_PredmetID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Profesor_ProfesorID, t.Predmet_PredmetID })
                .ForeignKey("dbo.Profesor", t => t.Profesor_ProfesorID, cascadeDelete: true)
                .ForeignKey("dbo.Predmet", t => t.Predmet_PredmetID, cascadeDelete: true)
                .Index(t => t.Profesor_ProfesorID)
                .Index(t => t.Predmet_PredmetID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfesorPredmet", "Predmet_PredmetID", "dbo.Predmet");
            DropForeignKey("dbo.ProfesorPredmet", "Profesor_ProfesorID", "dbo.Profesor");
            DropIndex("dbo.ProfesorPredmet", new[] { "Predmet_PredmetID" });
            DropIndex("dbo.ProfesorPredmet", new[] { "Profesor_ProfesorID" });
            DropTable("dbo.ProfesorPredmet");
        }
    }
}
