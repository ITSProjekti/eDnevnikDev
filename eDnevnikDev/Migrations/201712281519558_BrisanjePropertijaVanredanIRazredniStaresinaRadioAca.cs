namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrisanjePropertijaVanredanIRazredniStaresinaRadioAca : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Profesor", "Vanredan");
            DropColumn("dbo.Profesor", "RazredniStaresina");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profesor", "RazredniStaresina", c => c.Boolean(nullable: false));
            AddColumn("dbo.Profesor", "Vanredan", c => c.Boolean(nullable: false));
        }
    }
}
