namespace eDnevnikDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSprint4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArhivaOdeljenja",
                c => new
                    {
                        ArhivaOdeljenjaID = c.Int(nullable: false, identity: true),
                        OdeljenjeID = c.Int(nullable: false),
                        UcenikID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArhivaOdeljenjaID)
                .ForeignKey("dbo.Odeljenje", t => t.OdeljenjeID, cascadeDelete: true)
                .ForeignKey("dbo.Ucenik", t => t.UcenikID, cascadeDelete: true)
                .Index(t => t.OdeljenjeID)
                .Index(t => t.UcenikID);
            
            CreateTable(
                "dbo.Odeljenje",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OznakaID = c.Int(nullable: false),
                        StatusID = c.Int(nullable: false),
                        PocetakSkolskeGodine = c.Int(nullable: false),
                        KrajSkolskeGodine = c.Int(nullable: false),
                        Razred = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Oznaka", t => t.OznakaID, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusID, cascadeDelete: true)
                .Index(t => t.OznakaID)
                .Index(t => t.StatusID);
            
            CreateTable(
                "dbo.Cas",
                c => new
                    {
                        CasId = c.Int(nullable: false, identity: true),
                        Datum = c.DateTime(nullable: false),
                        Naziv = c.String(nullable: false),
                        Opis = c.String(nullable: false),
                        ProfesorId = c.Int(nullable: false),
                        PredmetId = c.Int(nullable: false),
                        OdeljenjeId = c.Int(nullable: false),
                        Polugodiste = c.Int(nullable: false),
                        Tromesecje = c.Int(nullable: false),
                        RedniBrojCasa = c.Int(nullable: false),
                        RedniBrojPredmeta = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CasId)
                .ForeignKey("dbo.Odeljenje", t => t.OdeljenjeId, cascadeDelete: true)
                .ForeignKey("dbo.Predmet", t => t.PredmetId, cascadeDelete: true)
                .ForeignKey("dbo.Profesor", t => t.ProfesorId, cascadeDelete: true)
                .Index(t => t.ProfesorId)
                .Index(t => t.PredmetId)
                .Index(t => t.OdeljenjeId);
            
            CreateTable(
                "dbo.Napomena",
                c => new
                    {
                        NapomenaId = c.Int(nullable: false, identity: true),
                        Opis = c.String(),
                        UcenikId = c.Int(nullable: false),
                        ProfesorId = c.Int(nullable: false),
                        CasId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NapomenaId)
                .ForeignKey("dbo.Cas", t => t.CasId, cascadeDelete: true)
                .ForeignKey("dbo.Profesor", t => t.ProfesorId, cascadeDelete: false)
                .ForeignKey("dbo.Ucenik", t => t.UcenikId, cascadeDelete: true)
                .Index(t => t.UcenikId)
                .Index(t => t.ProfesorId)
                .Index(t => t.CasId);
            
            CreateTable(
                "dbo.Profesor",
                c => new
                    {
                        ProfesorID = c.Int(nullable: false, identity: true),
                        Ime = c.String(nullable: false),
                        Prezime = c.String(nullable: false),
                        Telefon = c.String(nullable: false),
                        Adresa = c.String(nullable: false),
                        Vanredan = c.Boolean(nullable: false),
                        RazredniStaresina = c.Boolean(nullable: false),
                        RedniBroj = c.Int(nullable: false),
                        PromenaLozinke = c.Boolean(nullable: false),
                        UserProfesorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ProfesorID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserProfesorId)
                .Index(t => t.UserProfesorId);
            
            CreateTable(
                "dbo.Predmet",
                c => new
                    {
                        PredmetID = c.Int(nullable: false, identity: true),
                        NazivPredmeta = c.String(nullable: false),
                        TipOcenePredmetaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PredmetID)
                .ForeignKey("dbo.TipOcenePredmeta", t => t.TipOcenePredmetaId, cascadeDelete: true)
                .Index(t => t.TipOcenePredmetaId);
            
            CreateTable(
                "dbo.TipOcenePredmeta",
                c => new
                    {
                        TipOcenePredmetaId = c.Int(nullable: false, identity: true),
                        Tip = c.String(),
                    })
                .PrimaryKey(t => t.TipOcenePredmetaId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Ucenik",
                c => new
                    {
                        UcenikID = c.Int(nullable: false, identity: true),
                        ImeOca = c.String(nullable: false),
                        PrezimeOca = c.String(nullable: false),
                        ImeMajke = c.String(nullable: false),
                        PrezimeMajke = c.String(nullable: false),
                        Ime = c.String(nullable: false),
                        Prezime = c.String(nullable: false),
                        JMBG = c.String(nullable: false),
                        Adresa = c.String(nullable: false),
                        MestoPrebivalista = c.String(nullable: false),
                        BrojTelefonaRoditelja = c.String(nullable: false),
                        MestoRodjenjaId = c.Int(nullable: false),
                        Vanredan = c.Boolean(nullable: false),
                        RedniBroj = c.String(),
                        PromenaLozinke = c.Boolean(nullable: false),
                        SmerID = c.Int(nullable: false),
                        OdeljenjeId = c.Int(nullable: false),
                        Razred = c.Byte(nullable: false),
                        DatumRodjenja = c.DateTime(nullable: false),
                        JedinstveniBroj = c.String(),
                        Fotografija = c.Binary(),
                        BrojUDnevniku = c.Int(),
                        UserUcenikId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UcenikID)
                .ForeignKey("dbo.Grad", t => t.MestoRodjenjaId, cascadeDelete: true)
                .ForeignKey("dbo.Odeljenje", t => t.OdeljenjeId, cascadeDelete: false)
                .ForeignKey("dbo.Smer", t => t.SmerID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserUcenikId)
                .Index(t => t.MestoRodjenjaId)
                .Index(t => t.SmerID)
                .Index(t => t.OdeljenjeId)
                .Index(t => t.UserUcenikId);
            
            CreateTable(
                "dbo.Grad",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ocena",
                c => new
                    {
                        OcenaId = c.Int(nullable: false, identity: true),
                        Oznaka = c.Int(),
                        Plus = c.Boolean(),
                        UcenikId = c.Int(nullable: false),
                        CasId = c.Int(nullable: false),
                        TipOceneId = c.Int(nullable: false),
                        TipOpisneOceneId = c.Int(),
                        Napomena = c.String(),
                    })
                .PrimaryKey(t => t.OcenaId)
                .ForeignKey("dbo.Cas", t => t.CasId, cascadeDelete: true)
                .ForeignKey("dbo.TipOcene", t => t.TipOceneId, cascadeDelete: true)
                .ForeignKey("dbo.TipOpisneOcene", t => t.TipOpisneOceneId)
                .ForeignKey("dbo.Ucenik", t => t.UcenikId, cascadeDelete: true)
                .Index(t => t.UcenikId)
                .Index(t => t.CasId)
                .Index(t => t.TipOceneId)
                .Index(t => t.TipOpisneOceneId);
            
            CreateTable(
                "dbo.TipOcene",
                c => new
                    {
                        TipOceneId = c.Int(nullable: false, identity: true),
                        Tip = c.String(),
                    })
                .PrimaryKey(t => t.TipOceneId);
            
            CreateTable(
                "dbo.TipOpisneOcene",
                c => new
                    {
                        TipOpisneOceneId = c.Int(nullable: false, identity: true),
                        Tip = c.String(),
                    })
                .PrimaryKey(t => t.TipOpisneOceneId);
            
            CreateTable(
                "dbo.Odsustvo",
                c => new
                    {
                        OdsustvoId = c.Int(nullable: false, identity: true),
                        UcenikId = c.Int(nullable: false),
                        CasId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OdsustvoId)
                .ForeignKey("dbo.Cas", t => t.CasId, cascadeDelete: true)
                .ForeignKey("dbo.Ucenik", t => t.UcenikId, cascadeDelete: true)
                .Index(t => t.UcenikId)
                .Index(t => t.CasId);
            
            CreateTable(
                "dbo.Smer",
                c => new
                    {
                        SmerID = c.Int(nullable: false, identity: true),
                        NazivSmera = c.String(nullable: false),
                        Trajanje = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.SmerID);
            
            CreateTable(
                "dbo.Oznaka",
                c => new
                    {
                        OznakaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OznakaId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Opis = c.String(),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.PredmetOdeljenje",
                c => new
                    {
                        Predmet_PredmetID = c.Int(nullable: false),
                        Odeljenje_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Predmet_PredmetID, t.Odeljenje_Id })
                .ForeignKey("dbo.Predmet", t => t.Predmet_PredmetID, cascadeDelete: true)
                .ForeignKey("dbo.Odeljenje", t => t.Odeljenje_Id, cascadeDelete: true)
                .Index(t => t.Predmet_PredmetID)
                .Index(t => t.Odeljenje_Id);
            
            CreateTable(
                "dbo.PredmetProfesor",
                c => new
                    {
                        Predmet_PredmetID = c.Int(nullable: false),
                        Profesor_ProfesorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Predmet_PredmetID, t.Profesor_ProfesorID })
                .ForeignKey("dbo.Predmet", t => t.Predmet_PredmetID, cascadeDelete: true)
                .ForeignKey("dbo.Profesor", t => t.Profesor_ProfesorID, cascadeDelete: true)
                .Index(t => t.Predmet_PredmetID)
                .Index(t => t.Profesor_ProfesorID);
            
            CreateTable(
                "dbo.OznakaSmer",
                c => new
                    {
                        Oznaka_OznakaId = c.Int(nullable: false),
                        Smer_SmerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Oznaka_OznakaId, t.Smer_SmerID })
                .ForeignKey("dbo.Oznaka", t => t.Oznaka_OznakaId, cascadeDelete: true)
                .ForeignKey("dbo.Smer", t => t.Smer_SmerID, cascadeDelete: true)
                .Index(t => t.Oznaka_OznakaId)
                .Index(t => t.Smer_SmerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ArhivaOdeljenja", "UcenikID", "dbo.Ucenik");
            DropForeignKey("dbo.ArhivaOdeljenja", "OdeljenjeID", "dbo.Odeljenje");
            DropForeignKey("dbo.Odeljenje", "StatusID", "dbo.Status");
            DropForeignKey("dbo.Odeljenje", "OznakaID", "dbo.Oznaka");
            DropForeignKey("dbo.Cas", "ProfesorId", "dbo.Profesor");
            DropForeignKey("dbo.Cas", "PredmetId", "dbo.Predmet");
            DropForeignKey("dbo.Cas", "OdeljenjeId", "dbo.Odeljenje");
            DropForeignKey("dbo.Napomena", "UcenikId", "dbo.Ucenik");
            DropForeignKey("dbo.Ucenik", "UserUcenikId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ucenik", "SmerID", "dbo.Smer");
            DropForeignKey("dbo.OznakaSmer", "Smer_SmerID", "dbo.Smer");
            DropForeignKey("dbo.OznakaSmer", "Oznaka_OznakaId", "dbo.Oznaka");
            DropForeignKey("dbo.Odsustvo", "UcenikId", "dbo.Ucenik");
            DropForeignKey("dbo.Odsustvo", "CasId", "dbo.Cas");
            DropForeignKey("dbo.Ucenik", "OdeljenjeId", "dbo.Odeljenje");
            DropForeignKey("dbo.Ocena", "UcenikId", "dbo.Ucenik");
            DropForeignKey("dbo.Ocena", "TipOpisneOceneId", "dbo.TipOpisneOcene");
            DropForeignKey("dbo.Ocena", "TipOceneId", "dbo.TipOcene");
            DropForeignKey("dbo.Ocena", "CasId", "dbo.Cas");
            DropForeignKey("dbo.Ucenik", "MestoRodjenjaId", "dbo.Grad");
            DropForeignKey("dbo.Napomena", "ProfesorId", "dbo.Profesor");
            DropForeignKey("dbo.Profesor", "UserProfesorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Predmet", "TipOcenePredmetaId", "dbo.TipOcenePredmeta");
            DropForeignKey("dbo.PredmetProfesor", "Profesor_ProfesorID", "dbo.Profesor");
            DropForeignKey("dbo.PredmetProfesor", "Predmet_PredmetID", "dbo.Predmet");
            DropForeignKey("dbo.PredmetOdeljenje", "Odeljenje_Id", "dbo.Odeljenje");
            DropForeignKey("dbo.PredmetOdeljenje", "Predmet_PredmetID", "dbo.Predmet");
            DropForeignKey("dbo.Napomena", "CasId", "dbo.Cas");
            DropIndex("dbo.OznakaSmer", new[] { "Smer_SmerID" });
            DropIndex("dbo.OznakaSmer", new[] { "Oznaka_OznakaId" });
            DropIndex("dbo.PredmetProfesor", new[] { "Profesor_ProfesorID" });
            DropIndex("dbo.PredmetProfesor", new[] { "Predmet_PredmetID" });
            DropIndex("dbo.PredmetOdeljenje", new[] { "Odeljenje_Id" });
            DropIndex("dbo.PredmetOdeljenje", new[] { "Predmet_PredmetID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Odsustvo", new[] { "CasId" });
            DropIndex("dbo.Odsustvo", new[] { "UcenikId" });
            DropIndex("dbo.Ocena", new[] { "TipOpisneOceneId" });
            DropIndex("dbo.Ocena", new[] { "TipOceneId" });
            DropIndex("dbo.Ocena", new[] { "CasId" });
            DropIndex("dbo.Ocena", new[] { "UcenikId" });
            DropIndex("dbo.Ucenik", new[] { "UserUcenikId" });
            DropIndex("dbo.Ucenik", new[] { "OdeljenjeId" });
            DropIndex("dbo.Ucenik", new[] { "SmerID" });
            DropIndex("dbo.Ucenik", new[] { "MestoRodjenjaId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Predmet", new[] { "TipOcenePredmetaId" });
            DropIndex("dbo.Profesor", new[] { "UserProfesorId" });
            DropIndex("dbo.Napomena", new[] { "CasId" });
            DropIndex("dbo.Napomena", new[] { "ProfesorId" });
            DropIndex("dbo.Napomena", new[] { "UcenikId" });
            DropIndex("dbo.Cas", new[] { "OdeljenjeId" });
            DropIndex("dbo.Cas", new[] { "PredmetId" });
            DropIndex("dbo.Cas", new[] { "ProfesorId" });
            DropIndex("dbo.Odeljenje", new[] { "StatusID" });
            DropIndex("dbo.Odeljenje", new[] { "OznakaID" });
            DropIndex("dbo.ArhivaOdeljenja", new[] { "UcenikID" });
            DropIndex("dbo.ArhivaOdeljenja", new[] { "OdeljenjeID" });
            DropTable("dbo.OznakaSmer");
            DropTable("dbo.PredmetProfesor");
            DropTable("dbo.PredmetOdeljenje");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Status");
            DropTable("dbo.Oznaka");
            DropTable("dbo.Smer");
            DropTable("dbo.Odsustvo");
            DropTable("dbo.TipOpisneOcene");
            DropTable("dbo.TipOcene");
            DropTable("dbo.Ocena");
            DropTable("dbo.Grad");
            DropTable("dbo.Ucenik");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TipOcenePredmeta");
            DropTable("dbo.Predmet");
            DropTable("dbo.Profesor");
            DropTable("dbo.Napomena");
            DropTable("dbo.Cas");
            DropTable("dbo.Odeljenje");
            DropTable("dbo.ArhivaOdeljenja");
        }
    }
}
