namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopunjavanjeOdeljenjeSmer : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO OdeljenjeSmer VALUES(1,2)");
            Sql("INSERT INTO OdeljenjeSmer VALUES(2,2)");
            Sql("INSERT INTO OdeljenjeSmer VALUES(3,4)");
            Sql("INSERT INTO OdeljenjeSmer VALUES(4,1)");
            Sql("INSERT INTO OdeljenjeSmer VALUES(5,1)");
            Sql("INSERT INTO OdeljenjeSmer VALUES(6,3)");
            Sql("INSERT INTO OdeljenjeSmer VALUES(7,5)");
            Sql("INSERT INTO OdeljenjeSmer VALUES(7,6)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM OdeljenjeSmer");
        }
    }
}
