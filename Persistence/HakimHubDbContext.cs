using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Common;

namespace Persistence
{
    public class HakimHubDbContext : IdentityDbContext<AppUser>
    {
        public HakimHubDbContext(DbContextOptions<HakimHubDbContext> options)
         : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            CreateCalculateDistanceFunction(); // Call the function during the DbContext constructor.
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HakimHubDbContext).Assembly);

            // Unqiue service name
            modelBuilder.Entity<Service>()
             .HasIndex(s => s.ServiceName)
             .IsUnique();

            modelBuilder.Entity<Speciality>()
           .HasIndex(s => s.Name)
           .IsUnique();

            // address to institution

            modelBuilder.Entity<InstitutionProfile>()
            .HasOne(p => p.Address)
            .WithOne(a => a.Institution)
            .HasForeignKey<Address>(a => a.InstitutionId)
            .OnDelete(DeleteBehavior.Cascade);

            // doctor profile to phot
            modelBuilder.Entity<DoctorProfile>()
                .HasOne(d => d.Photo)
                .WithOne()
                .HasForeignKey<Photo>(x => x.DoctorProfileId);


            // institiution logo with photo
            modelBuilder.Entity<InstitutionProfile>()
                .HasOne(d => d.Logo)
                .WithOne()
                .HasForeignKey<Photo>(x => x.LogoId);

            // institiution banner with photo
            modelBuilder.Entity<InstitutionProfile>()
                .HasOne(d => d.Banner)
                .WithOne()
                .HasForeignKey<Photo>(x => x.BannerId);
            //
            modelBuilder.Entity<DoctorProfile>()
                .HasOne(d => d.MainInstitution)
                .WithMany()
                .HasForeignKey(d => d.MainInstitutionId)
                .OnDelete(DeleteBehavior.Restrict);

            // doctor availability
            modelBuilder.Entity<DoctorProfile>()
            .HasMany(e => e.DoctorAvailabilities)
            .WithOne(d => d.Doctor)
            .HasForeignKey(e => e.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

            // doctor to institution
            modelBuilder.Entity<DoctorProfile>()
            .HasMany(e => e.Institutions)
            .WithMany(d => d.Doctors);

            // education availability
            modelBuilder.Entity<DoctorProfile>()
            .HasMany(e => e.Educations)
            .WithOne(d => d.Doctor)
            .HasForeignKey(e => e.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

            // experience availability
            modelBuilder.Entity<DoctorProfile>()
            .HasMany(e => e.Experiences)
            .WithOne(d => d.Doctor)
            .HasForeignKey(e => e.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

            // speciality availability
            modelBuilder.Entity<DoctorProfile>()
            .HasMany(e => e.Specialities)
            .WithMany(d => d.Doctors);
            // 
            modelBuilder.Entity<DoctorProfile>()
            .HasOne(e => e.Photo)
            .WithOne()
            .HasForeignKey<DoctorProfile>(e => e.PhotoId)
            .OnDelete(DeleteBehavior.Cascade);

            // 
            modelBuilder.Entity<Education>()
            .HasOne(e => e.EducationInstitutionLogo)
            .WithOne()
            .HasForeignKey<Education>(e => e.EducationInstitutionLogoId)
            .OnDelete(DeleteBehavior.Cascade);
            // 
            modelBuilder.Entity<InstitutionProfile>()
            .HasOne(e => e.InstitutionAvailability)
            .WithOne(d => d.Institution)
            .HasForeignKey<InstitutionAvailability>(e => e.InstitutionId)
            .OnDelete(DeleteBehavior.Cascade);

            // address to institution
            modelBuilder.Entity<InstitutionProfile>()
            .HasMany(e => e.Services)
            .WithMany(d => d.Institutions);

            // address to institution
            modelBuilder.Entity<InstitutionProfile>()
            .HasMany(e => e.Photos)
            .WithOne()
            .HasForeignKey("InstitutionProfileId")
            .OnDelete(DeleteBehavior.Cascade);

            // address to institution
            modelBuilder.Entity<InstitutionProfile>()
            .HasOne(e => e.Banner)
            .WithOne()
            .HasForeignKey<InstitutionProfile>(e => e.BannerId)
            .OnDelete(DeleteBehavior.Cascade);

            // address to institution
            modelBuilder.Entity<InstitutionProfile>()
            .HasOne(e => e.Logo)
            .WithOne()
            .HasForeignKey<InstitutionProfile>(e => e.LogoId)
            .OnDelete(DeleteBehavior.Cascade);

        
        }


        private void CreateCalculateDistanceFunction()
        {
            Database.ExecuteSqlRaw(@"
                CREATE OR REPLACE FUNCTION calculate_distance(
                    lat1 double precision,
                    lon1 double precision,
                    lat2 double precision,
                    lon2 double precision
                )
                RETURNS double precision AS $$
                DECLARE
                    x double precision = 69.1 * (lat2 - lat1);
                    y double precision = 69.1 * (lon2 - lon1) * cos(radians(lat1));
                    distance double precision = sqrt(x * x + y * y);
                BEGIN
                    RETURN distance;
                END;
                $$ LANGUAGE plpgsql;
            ");
        }
       

        [DbFunction("calculate_distance", "public")]
        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var result = Database.ExecuteSqlInterpolated($"SELECT calculate_distance({lat1}, {lon1}, {lat2}, {lon2})");
            return Convert.ToDouble(result);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var entry in ChangeTracker.Entries<BaseDomainEntity>())
            {
                entry.Entity.LastModifiedDate = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<DoctorProfile> DoctorProfiles { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<InstitutionProfile> InstitutioProfiles { get; set; }
        public DbSet<InstitutionAvailability> InstitutionAvailabilities { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
    }
}

