namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatBrojTelefonaRoditeljaStaratelja : DbMigration
    {
        public override void Up()
        {
            
            
            AddColumn("dbo.Ucenik", "BrojTelefonaRoditelja", c => c.String(nullable: false));
            
        }
        
        public override void Down()
        {
            
            DropColumn("dbo.Ucenik", "BrojTelefonaRoditelja");

        }
    }
}
