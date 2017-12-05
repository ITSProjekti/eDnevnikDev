namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatPolUModeluProfesor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profesor", "PolId", c => c.Int(nullable: false));
            CreateIndex("dbo.Profesor", "PolId");
            AddForeignKey("dbo.Profesor", "PolId", "dbo.Pol", "PolId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Profesor", "PolId", "dbo.Pol");
            DropIndex("dbo.Profesor", new[] { "PolId" });
            DropColumn("dbo.Profesor", "PolId");
        }
    }
}
