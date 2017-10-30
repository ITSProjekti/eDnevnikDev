namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class backupnecega : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ocena", "TipOpisneOceneId", "dbo.TipOpisneOcene");
            DropIndex("dbo.Ocena", new[] { "TipOpisneOceneId" });
            AlterColumn("dbo.Ocena", "Oznaka", c => c.Int());
            AlterColumn("dbo.Ocena", "Plus", c => c.Boolean());
            AlterColumn("dbo.Ocena", "TipOpisneOceneId", c => c.Int());
            CreateIndex("dbo.Ocena", "TipOpisneOceneId");
            AddForeignKey("dbo.Ocena", "TipOpisneOceneId", "dbo.TipOpisneOcene", "TipOpisneOceneId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ocena", "TipOpisneOceneId", "dbo.TipOpisneOcene");
            DropIndex("dbo.Ocena", new[] { "TipOpisneOceneId" });
            AlterColumn("dbo.Ocena", "TipOpisneOceneId", c => c.Int(nullable: false));
            AlterColumn("dbo.Ocena", "Plus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Ocena", "Oznaka", c => c.Int(nullable: false));
            CreateIndex("dbo.Ocena", "TipOpisneOceneId");
            AddForeignKey("dbo.Ocena", "TipOpisneOceneId", "dbo.TipOpisneOcene", "TipOpisneOceneId", cascadeDelete: true);
        }
    }
}
