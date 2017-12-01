namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UcenikModelSmerIOdeljenjeUNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje");
            DropForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer");
            DropIndex("dbo.Ucenik", new[] { "SmerID" });
            DropIndex("dbo.Ucenik", new[] { "OdeljenjeId" });
            AlterColumn("dbo.Ucenik", "SmerID", c => c.Int());
            AlterColumn("dbo.Ucenik", "OdeljenjeId", c => c.Int());
            CreateIndex("dbo.Ucenik", "SmerID");
            CreateIndex("dbo.Ucenik", "OdeljenjeId");
            AddForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje", "Id");
            AddForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer", "SmerID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer");
            DropForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje");
            DropIndex("dbo.Ucenik", new[] { "OdeljenjeId" });
            DropIndex("dbo.Ucenik", new[] { "SmerID" });
            AlterColumn("dbo.Ucenik", "OdeljenjeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Ucenik", "SmerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Ucenik", "OdeljenjeId");
            CreateIndex("dbo.Ucenik", "SmerID");
            AddForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer", "SmerID", cascadeDelete: true);
            AddForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje", "Id", cascadeDelete: true);
        }
    }
}
