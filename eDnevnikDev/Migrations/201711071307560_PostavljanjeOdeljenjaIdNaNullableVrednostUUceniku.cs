namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostavljanjeOdeljenjaIdNaNullableVrednostUUceniku : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje");
            DropIndex("dbo.Ucenik", new[] { "OdeljenjeId" });
            AlterColumn("dbo.Ucenik", "OdeljenjeId", c => c.Int());
            CreateIndex("dbo.Ucenik", "OdeljenjeId");
            AddForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje");
            DropIndex("dbo.Ucenik", new[] { "OdeljenjeId" });
            AlterColumn("dbo.Ucenik", "OdeljenjeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ucenik", "OdeljenjeId");
            AddForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje", "Id", cascadeDelete: true);
        }
    }
}
