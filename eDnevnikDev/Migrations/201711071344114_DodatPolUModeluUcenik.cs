namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatPolUModeluUcenik : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pol",
                c => new
                    {
                        PolId = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                    })
                .PrimaryKey(t => t.PolId);
            
            AddColumn("dbo.Ucenik", "PolId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ucenik", "PolId");
            AddForeignKey("dbo.Ucenik", "PolId", "dbo.Pol", "PolId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ucenik", "PolId", "dbo.Pol");
            DropIndex("dbo.Ucenik", new[] { "PolId" });
            DropColumn("dbo.Ucenik", "PolId");
            DropTable("dbo.Pol");
        }
    }
}
