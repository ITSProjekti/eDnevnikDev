using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace eDnevnikDev.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Ucenik> Ucenici { get; set; }
        public virtual DbSet<ArhivaOdeljenja> ArhivaOdeljenja { get; set; }
        public virtual DbSet<Grad> Gradovi {get;set;}
        public virtual DbSet<Profesor> Profesori { get; set; }
        public virtual DbSet<Smer> Smerovi { get; set; }
        public virtual DbSet<Predmet> Predmeti { get; set; }
        public virtual DbSet<Odeljenje> Odeljenja { get; set; }
        public virtual DbSet<Odsustvo> Odsustva { get; set; }
        public virtual DbSet<Napomena> Napomene { get; set; }
        public virtual DbSet<Cas> Casovi { get; set; }
        public virtual DbSet<Ocena> Ocene { get; set; }
        public virtual DbSet<TipOcene> TipoviOcena { get; set; }
        public virtual DbSet<TipOpisneOcene> TipoviOpisnihOcena { get; set; }
        public virtual DbSet<Status> Statusi{ get; set; }
        public virtual DbSet<Oznaka> Oznake { get; set; }
        public virtual DbSet<TipOcenePredmeta> TipoviOcenaPredmeta { get; set; }


        public ApplicationDbContext()
            : base("eDnevnik", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder); 
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}