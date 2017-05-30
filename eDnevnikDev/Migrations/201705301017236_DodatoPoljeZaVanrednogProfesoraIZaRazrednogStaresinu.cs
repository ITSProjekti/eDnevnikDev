namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatoPoljeZaVanrednogProfesoraIZaRazrednogStaresinu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profesor", "Vanredan", c => c.Boolean(nullable: false));
            AddColumn("dbo.Profesor", "RazredniStaresina", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profesor", "RazredniStaresina");
            DropColumn("dbo.Profesor", "Vanredan");
        }
    }
}
