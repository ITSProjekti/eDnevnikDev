namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopunjavanjeSmerOznaka : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO SmerOznaka VALUES(2,1)");
            Sql("INSERT INTO SmerOznaka VALUES(2,2)");
            Sql("INSERT INTO SmerOznaka VALUES(4,3)");
            Sql("INSERT INTO SmerOznaka VALUES(1,4)");
            Sql("INSERT INTO SmerOznaka VALUES(1,5)");
            Sql("INSERT INTO SmerOznaka VALUES(3,6)");
            Sql("INSERT INTO SmerOznaka VALUES(5,7)");
            Sql("INSERT INTO SmerOznaka VALUES(6,7)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM SmerOznaka WHERE Oznaka_OznakaId BETWEEN 1 AND 7");
        }
    }
}
