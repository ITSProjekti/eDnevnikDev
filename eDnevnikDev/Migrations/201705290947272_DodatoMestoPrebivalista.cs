namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatoMestoPrebivalista : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "MestoPrebivalista", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "MestoPrebivalista");
        }
    }
}
