namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatoMestoRodjenjaUModelUcenik : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "MestoRodjenjaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ucenik", "MestoRodjenjaId");
            AddForeignKey("dbo.Ucenik", "MestoRodjenjaId", "dbo.Grad", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ucenik", "MestoRodjenjaId", "dbo.Grad");
            DropIndex("dbo.Ucenik", new[] { "MestoRodjenjaId" });
            DropColumn("dbo.Ucenik", "MestoRodjenjaId");
        }
    }
}
