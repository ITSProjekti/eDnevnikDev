namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodataPromenaLozinkeUUcenikuIProfi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profesor", "PromenaLozinke", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ucenik", "PromenaLozinke", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "PromenaLozinke");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "PromenaLozinke", c => c.Boolean(nullable: false));
            DropColumn("dbo.Ucenik", "PromenaLozinke");
            DropColumn("dbo.Profesor", "PromenaLozinke");
        }
    }
}
