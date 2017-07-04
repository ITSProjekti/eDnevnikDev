namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopunjavanjeStatusaOdeljenja : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Status VALUES('Arhivirano')");
            Sql("INSERT INTO Status VALUES('U toku')");
            Sql("INSERT INTO Status VALUES('Kreirano')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Status WHERE Opis = 'Arhivirano'");
            Sql("DELETE FROM Status WHERE Opis = 'U toku'");
            Sql("DELETE FROM Status WHERE Opis = 'Kreirano'");

        }
    }
}
