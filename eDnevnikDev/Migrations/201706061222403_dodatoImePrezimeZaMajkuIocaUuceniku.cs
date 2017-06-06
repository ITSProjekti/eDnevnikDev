namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodatoImePrezimeZaMajkuIocaUuceniku : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "ImeOca", c => c.String(nullable: false));
            AddColumn("dbo.Ucenik", "PrezimeOca", c => c.String(nullable: false));
            AddColumn("dbo.Ucenik", "ImeMajke", c => c.String(nullable: false));
            AddColumn("dbo.Ucenik", "PrezimeMajke", c => c.String(nullable: false));
            DropColumn("dbo.Ucenik", "RoditeljStaratelj");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ucenik", "RoditeljStaratelj", c => c.String(nullable: false));
            DropColumn("dbo.Ucenik", "PrezimeMajke");
            DropColumn("dbo.Ucenik", "ImeMajke");
            DropColumn("dbo.Ucenik", "PrezimeOca");
            DropColumn("dbo.Ucenik", "ImeOca");
        }
    }
}
