namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodataFotografijaUModelulProfesor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profesor", "Fotografija", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profesor", "Fotografija");
        }
    }
}
