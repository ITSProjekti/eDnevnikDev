namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StatusUcenika : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatusUcenika",
                c => new
                    {
                        StatusUcenikaId = c.Int(nullable: false, identity: true),
                        Opis = c.String(),
                    })
                .PrimaryKey(t => t.StatusUcenikaId);
            
            AddColumn("dbo.Ucenik", "StatusUcenikaId", c => c.Int());
            CreateIndex("dbo.Ucenik", "StatusUcenikaId");
            AddForeignKey("dbo.Ucenik", "StatusUcenikaId", "dbo.StatusUcenika", "StatusUcenikaId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ucenik", "StatusUcenikaId", "dbo.StatusUcenika");
            DropIndex("dbo.Ucenik", new[] { "StatusUcenikaId" });
            DropColumn("dbo.Ucenik", "StatusUcenikaId");
            DropTable("dbo.StatusUcenika");
        }
    }
}
