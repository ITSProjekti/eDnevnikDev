namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UcenikModelRequiredZaNekaPolja : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer");
            DropIndex("dbo.Ucenik", new[] { "SmerID" });
            AlterColumn("dbo.Ucenik", "SmerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Ucenik", "SmerID");
            AddForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer", "SmerID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer");
            DropIndex("dbo.Ucenik", new[] { "SmerID" });
            AlterColumn("dbo.Ucenik", "SmerID", c => c.Int());
            CreateIndex("dbo.Ucenik", "SmerID");
            AddForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer", "SmerID");
        }
    }
}
