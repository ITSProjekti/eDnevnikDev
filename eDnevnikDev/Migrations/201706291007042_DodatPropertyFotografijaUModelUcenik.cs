namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodatPropertyFotografijaUModelUcenik : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "Fotografija", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "Fotografija");
        }
    }
}
