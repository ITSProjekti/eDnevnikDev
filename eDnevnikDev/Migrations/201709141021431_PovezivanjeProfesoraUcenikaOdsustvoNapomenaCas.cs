namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PovezivanjeProfesoraUcenikaOdsustvoNapomenaCas : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProfesorPredmet", newName: "PredmetProfesor");
            DropPrimaryKey("dbo.PredmetProfesor");
            AddColumn("dbo.Napomena", "UcenikId", c => c.Int(nullable: false));
            AddColumn("dbo.Napomena", "ProfesorId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.PredmetProfesor", new[] { "Predmet_PredmetID", "Profesor_ProfesorID" });
            CreateIndex("dbo.Napomena", "UcenikId");
            CreateIndex("dbo.Napomena", "ProfesorId");
            AddForeignKey("dbo.Napomena", "ProfesorId", "dbo.Profesor", "ProfesorID", cascadeDelete: false);
            AddForeignKey("dbo.Napomena", "UcenikId", "dbo.Ucenik", "UcenikID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Napomena", "UcenikId", "dbo.Ucenik");
            DropForeignKey("dbo.Napomena", "ProfesorId", "dbo.Profesor");
            DropIndex("dbo.Napomena", new[] { "ProfesorId" });
            DropIndex("dbo.Napomena", new[] { "UcenikId" });
            DropPrimaryKey("dbo.PredmetProfesor");
            DropColumn("dbo.Napomena", "ProfesorId");
            DropColumn("dbo.Napomena", "UcenikId");
            AddPrimaryKey("dbo.PredmetProfesor", new[] { "Profesor_ProfesorID", "Predmet_PredmetID" });
            RenameTable(name: "dbo.PredmetProfesor", newName: "ProfesorPredmet");
        }
    }
}
