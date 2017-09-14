namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class acaIdentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ucenik", "UserUcenikId", c => c.String(maxLength: 128));
            AddColumn("dbo.Profesor", "UserProfesorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Ucenik", "UserUcenikId");
            CreateIndex("dbo.Profesor", "UserProfesorId");
            AddForeignKey("dbo.Ucenik", "UserUcenikId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Profesor", "UserProfesorId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Profesor", "UserProfesorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ucenik", "UserUcenikId", "dbo.AspNetUsers");
            DropIndex("dbo.Profesor", new[] { "UserProfesorId" });
            DropIndex("dbo.Ucenik", new[] { "UserUcenikId" });
            DropColumn("dbo.Profesor", "UserProfesorId");
            DropColumn("dbo.Ucenik", "UserUcenikId");
        }
    }
}
