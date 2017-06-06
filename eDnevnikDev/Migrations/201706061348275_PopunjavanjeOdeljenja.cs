namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopunjavanjeOdeljenja : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Odeljenje(Oznaka, SmerID) VALUES(1, 2)");
            Sql("INSERT INTO Odeljenje(Oznaka, SmerID) VALUES(2, 2)");
            Sql("INSERT INTO Odeljenje(Oznaka, SmerID) VALUES(3, 4)");
            Sql("INSERT INTO Odeljenje(Oznaka, SmerID) VALUES(4, 1)");
            Sql("INSERT INTO Odeljenje(Oznaka, SmerID) VALUES(5, 1)");
            Sql("INSERT INTO Odeljenje(Oznaka, SmerID) VALUES(6, 3)");
            Sql("INSERT INTO Odeljenje(Oznaka, SmerID) VALUES(7, 5)");



        }

        public override void Down()
        {
            Sql("DELETE FROM Odeljenje");

        }
    }
}
