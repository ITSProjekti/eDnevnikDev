namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatDbSetZaOdeljenja : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OdeljenjeSmer", newName: "SmerOdeljenje");
            DropPrimaryKey("dbo.SmerOdeljenje");
            AddPrimaryKey("dbo.SmerOdeljenje", new[] { "Smer_SmerID", "Odeljenje_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.SmerOdeljenje");
            AddPrimaryKey("dbo.SmerOdeljenje", new[] { "Odeljenje_Id", "Smer_SmerID" });
            RenameTable(name: "dbo.SmerOdeljenje", newName: "OdeljenjeSmer");
        }
    }
}
