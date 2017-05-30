namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatModelSmer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Smer",
                c => new
                    {
                        SmerID = c.Int(nullable: false, identity: true),
                        NazivSmera = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SmerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Smer");
        }
    }
}
