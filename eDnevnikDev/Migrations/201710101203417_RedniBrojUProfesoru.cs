namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RedniBrojUProfesoru : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profesor", "RedniBroj", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "PromenaLozinke", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PromenaLozinke");
            DropColumn("dbo.Profesor", "RedniBroj");
        }
    }
}
