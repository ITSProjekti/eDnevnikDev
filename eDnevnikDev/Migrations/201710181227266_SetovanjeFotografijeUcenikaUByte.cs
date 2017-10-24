namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetovanjeFotografijeUcenikaUByte : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Ucenik", "Fotografija");
            AddColumn("dbo.Ucenik", "Fotografija", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ucenik", "Fotografija");
            AddColumn("dbo.Ucenik", "Fotografija", c => c.String());
        }
    }
}
