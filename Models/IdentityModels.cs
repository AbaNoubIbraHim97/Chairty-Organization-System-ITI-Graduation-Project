using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string UserType { get; set; }
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
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(null); // Remove default initializer
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientSurgery> PatientSurgeries { get; set; }
        public DbSet<PatientDisease> patientDiseases { get; set; }
        public DbSet<Abanoub> Abanoubs { get; set; }
        public DbSet<TemporaryCashing> TemporaryCashings { get; set; }
        public DbSet<MonthlyCaching> MonthlyCachings { get; set; }
        public DbSet<PatientMedicineTest> PatientMedicineTests { get; set; }
        public DbSet<Diseases> Diseases { get; set; }
        public DbSet<Medicines> Medicines { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Donations> Donations { get; set; }
        public DbSet<Surgery> Surgeries { get; set; }
        public DbSet<MedicinesCategory> MedicinesCategories { get; set; }

       
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<WebApplication1.Models.RoleModel> RoleModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Conventions.Remove<PluralizingTableNameConvention>();
            // Primary keys
            builder.Entity<Patient>().HasKey(q => q.ID);
            builder.Entity<Medicines>().HasKey(q => q.ID);
            builder.Entity<PatientMedicineTest>().HasKey(q =>
                new {
                    q.PatientID,
                    q.MedicineID
                });

            // Relationships
            builder.Entity<PatientMedicineTest>()
                .HasRequired(t => t.Patient)
                .WithMany(t => t.PatientMedicineTests)
                .HasForeignKey(t => t.PatientID);

            builder.Entity<PatientMedicineTest>()
                .HasRequired(t => t.Medicines)
                .WithMany(t => t.PatientMedicineTests)
                .HasForeignKey(t => t.MedicineID);
        }

        //public DbSet<UserTest> UserTests { get; set; }
        //public DbSet<EmailTest> EmailTests { get; set; }
        //public DbSet<UserEmail> UserEmails { get; set; }
    }
}