namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopunjavanjeGradova : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Grad(Naziv) VALUES('Beograd')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Valjevo')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Vranje')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Vršac')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Zaječar')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Zrenjanin')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Jagodina')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Kikinda')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Kragujevac')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Kraljevo')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Kruševac')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Leskovac')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Loznica')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Niš')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Novi Pazar')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Novi Sad')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Pančevo')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Pirot')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Požarevac')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Priština')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Svilajnac')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Smederevo')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Sombor')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Sremska Mitrovica')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Subotica')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Užice')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Čačak')");
            Sql("INSERT INTO Grad(Naziv) VALUES('Šabac')");


        }

        public override void Down()
        {
        }
    }
}
