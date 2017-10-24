namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrisanjeGodineUpisaUUceniku : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Ucenik", "GodinaUpisa");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ucenik", "GodinaUpisa", c => c.DateTime(nullable: false));
        }
    }
}
