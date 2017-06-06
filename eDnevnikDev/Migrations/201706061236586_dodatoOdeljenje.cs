namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodatoOdeljenje : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Odeljenje",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Oznaka = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Ucenik", "OdeljenjeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ucenik", "OdeljenjeId");
            AddForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje");
            DropIndex("dbo.Ucenik", new[] { "OdeljenjeId" });
            DropColumn("dbo.Ucenik", "OdeljenjeId");
            DropTable("dbo.Odeljenje");
        }
    }
}
