namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopunjavanjeGradova : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Beograd')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Valjevo')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Vranje')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Vršac')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Zaječar')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Zrenjanin')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Jagodina')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Kikinda')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Kragujevac')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Kraljevo')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Kruševac')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Leskovac')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Loznica')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Niš')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Novi Pazar')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Novi Sad')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Pančevo')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Pirot')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Požarevac')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Priština')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Svilajnac')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Smederevo')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Sombor')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Sremska Mitrovica')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Subotica')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Užice')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Čačak')");
            Sql("INSERT INTO Grad(Naziv) VALUES(N'Šabac')");


        }

        public override void Down()
        {
            Sql("Delete from Grad");
        }
    }
}
