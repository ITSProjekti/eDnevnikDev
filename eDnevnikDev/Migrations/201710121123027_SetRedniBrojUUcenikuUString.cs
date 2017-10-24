namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetRedniBrojUUcenikuUString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ucenik", "RedniBroj", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ucenik", "RedniBroj", c => c.Int(nullable: false));
        }
    }
}
