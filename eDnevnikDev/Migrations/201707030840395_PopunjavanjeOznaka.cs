namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopunjavanjeOznaka : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Oznaka VALUES(1)");
            Sql("INSERT INTO Oznaka VALUES(2)");
            Sql("INSERT INTO Oznaka VALUES(3)");
            Sql("INSERT INTO Oznaka VALUES(4)");
            Sql("INSERT INTO Oznaka VALUES(5)");
            Sql("INSERT INTO Oznaka VALUES(6)");
            Sql("INSERT INTO Oznaka VALUES(7)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Oznaka WHERE OznakaId BETWEEN 1 AND 7");
        }
    }
}
