namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IzbacenGradDodatoMestoRodjenjaRucnoDodatPropertiUDomuRadioAca : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ucenik", "MestoRodjenjaId", "dbo.Grad");
            DropIndex("dbo.Ucenik", new[] { "MestoRodjenjaId" });
            AddColumn("dbo.Ucenik", "MestoRodjenja", c => c.String(nullable: false));
            AddColumn("dbo.Ucenik", "UDomu", c => c.Boolean(nullable: false));
            DropColumn("dbo.Ucenik", "MestoRodjenjaId");
            DropTable("dbo.Grad");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Grad",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Ucenik", "MestoRodjenjaId", c => c.Int(nullable: false));
            DropColumn("dbo.Ucenik", "UDomu");
            DropColumn("dbo.Ucenik", "MestoRodjenja");
            CreateIndex("dbo.Ucenik", "MestoRodjenjaId");
            AddForeignKey("dbo.Ucenik", "MestoRodjenjaId", "dbo.Grad", "Id", cascadeDelete: true);
        }
    }
}
