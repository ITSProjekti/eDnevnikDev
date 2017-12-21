namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datumIspisaUNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ucenik", "DatumIspisa", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ucenik", "DatumIspisa", c => c.DateTime(nullable: false));
        }
    }
}
