namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelaCas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cas", "Polugodiste", c => c.Int(nullable: false));
            AddColumn("dbo.Cas", "Tromesecje", c => c.Int(nullable: false));
            AddColumn("dbo.Cas", "RedniBrojCasa", c => c.Int(nullable: false));
            AddColumn("dbo.Cas", "RedniBrojPredmeta", c => c.Int(nullable: false));
            AlterColumn("dbo.Cas", "Opis", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cas", "Opis", c => c.String());
            DropColumn("dbo.Cas", "RedniBrojPredmeta");
            DropColumn("dbo.Cas", "RedniBrojCasa");
            DropColumn("dbo.Cas", "Tromesecje");
            DropColumn("dbo.Cas", "Polugodiste");
        }
    }
}
