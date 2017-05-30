namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodatModelProfesora : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Profesor",
                c => new
                    {
                        ProfesorID = c.Int(nullable: false, identity: true),
                        Ime = c.String(nullable: false),
                        Prezime = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ProfesorID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Profesor");
        }
    }
}
