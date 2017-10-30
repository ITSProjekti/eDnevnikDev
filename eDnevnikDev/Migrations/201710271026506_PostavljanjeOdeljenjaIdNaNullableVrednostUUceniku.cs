namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostavljanjeOdeljenjaIdNaNullableVrednostUUceniku : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ucenik", "OdeljenjeId", c=>c.Int(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ucenik", "OdeljenjeId", c => c.Int());

        }
    }
}
