namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatModelGrad : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Grad",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Ucenik", "JMBG", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ucenik", "JMBG", c => c.String(nullable: false));
            DropTable("dbo.Grad");
        }
    }
}
