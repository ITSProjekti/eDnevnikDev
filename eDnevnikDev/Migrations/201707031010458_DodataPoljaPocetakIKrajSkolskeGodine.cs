namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodataPoljaPocetakIKrajSkolskeGodine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Odeljenje", "PocetakSkolskeGodine", c => c.Int(nullable: false));
            AddColumn("dbo.Odeljenje", "KrajSkolskeGodine", c => c.Int(nullable: false));
            DropColumn("dbo.Odeljenje", "SkolskaGodina");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Odeljenje", "SkolskaGodina", c => c.DateTime(nullable: false));
            DropColumn("dbo.Odeljenje", "KrajSkolskeGodine");
            DropColumn("dbo.Odeljenje", "PocetakSkolskeGodine");
        }
    }
}
