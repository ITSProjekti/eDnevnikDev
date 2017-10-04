namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tipOcenePredmeta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipOcenePredmeta",
                c => new
                    {
                        TipOcenePredmetaId = c.Int(nullable: false, identity: true),
                        Tip = c.String(),
                    })
                .PrimaryKey(t => t.TipOcenePredmetaId);
            
            AddColumn("dbo.Predmet", "TipOcenePredmetaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Predmet", "TipOcenePredmetaId");
            AddForeignKey("dbo.Predmet", "TipOcenePredmetaId", "dbo.TipOcenePredmeta", "TipOcenePredmetaId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Predmet", "TipOcenePredmetaId", "dbo.TipOcenePredmeta");
            DropIndex("dbo.Predmet", new[] { "TipOcenePredmetaId" });
            DropColumn("dbo.Predmet", "TipOcenePredmetaId");
            DropTable("dbo.TipOcenePredmeta");
        }
    }
}
