namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatBrojUDnevnikuUModeluUcenik : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "BrojUDnevniku", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "BrojUDnevniku");
        }
    }
}
