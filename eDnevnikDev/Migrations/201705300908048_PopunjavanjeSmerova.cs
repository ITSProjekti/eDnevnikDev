namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopunjavanjeSmerova : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Smer(NazivSmera) VALUES(N'Veterinarski tehničar')");
            Sql("INSERT INTO Smer(NazivSmera) VALUES(N'Poljoprivredni tehničar')");
            Sql("INSERT INTO Smer(NazivSmera) VALUES(N'Mesar')");
            Sql("INSERT INTO Smer(NazivSmera) VALUES(N'Prehrambeni tehničar')");
            Sql("INSERT INTO Smer(NazivSmera) VALUES(N'Tehničar hortikulture')");
            Sql("INSERT INTO Smer(NazivSmera) VALUES(N'Rukovalac-mehaničar privredne tehnike')");

        }

        public override void Down()
        {
            Sql("DELETE FROM Smer");

        }
    }
}
