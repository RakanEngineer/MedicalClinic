using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.ManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<ApplicationUser>(builder =>
            {
                builder.Property(user => user.DisplayName)
                    .HasMaxLength(100);

                builder.Property(user => user.RefreshToken)
                    .HasMaxLength(512);
            });
        }
    }
}
