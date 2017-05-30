namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatModelPredmet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Predmet",
                c => new
                    {
                        PredmetID = c.Int(nullable: false, identity: true),
                        NazivPredmeta = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PredmetID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Predmet");
        }
    }
}
