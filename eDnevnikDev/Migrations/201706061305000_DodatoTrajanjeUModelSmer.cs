namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatoTrajanjeUModelSmer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Smer", "Trajanje", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Smer", "Trajanje");
        }
    }
}
