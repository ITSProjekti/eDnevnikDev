namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PovezaniModeliOdeljenjeSmer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "Razred", c => c.Byte(nullable: false));
            AddColumn("dbo.Odeljenje", "SmerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Odeljenje", "SmerID");
            AddForeignKey("dbo.Odeljenje", "SmerID", "dbo.Smer", "SmerID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Odeljenje", "SmerID", "dbo.Smer");
            DropIndex("dbo.Odeljenje", new[] { "SmerID" });
            DropColumn("dbo.Odeljenje", "SmerID");
            DropColumn("dbo.Ucenik", "Razred");
        }
    }
}
