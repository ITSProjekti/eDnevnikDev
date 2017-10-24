namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GodinaUpisaUUceniku : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "GodinaUpisa", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "GodinaUpisa");
        }
    }
}
