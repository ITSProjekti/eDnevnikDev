namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatSmeruModelUcenik : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "SmerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Ucenik", "SmerID");
            AddForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer", "SmerID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer");
            DropIndex("dbo.Ucenik", new[] { "SmerID" });
            DropColumn("dbo.Ucenik", "SmerID");
        }
    }
}
