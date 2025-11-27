using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Offer> Offers { get; set; }


        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();*/

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(e => e.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<JobApplication>(entity =>
            { 
                entity.Property(e => e.Company).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Position).HasMaxLength(100).IsRequired();
                entity.Property(e => e.SalaryExpectation).HasPrecision(18,2);

                entity.HasOne(u => u.User).WithMany(j => j.JobApplications)
                .HasForeignKey(u=>u.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Interview>(entity =>
            {
                entity.Property(e => e.RoundName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.LocationUrl).HasMaxLength(500);

                entity.HasOne(j => j.JobApplication).WithMany(i =>i.Interviews)
                .HasForeignKey(a => a.ApplicationId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.Property(e => e.Salary).HasPrecision(18, 2);

                entity.HasOne(j => j.JobApplication).WithOne(o => o.Offer)
                .HasForeignKey<Offer>(a=>a.ApplicationId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<RefreshToken>()
            .HasIndex(r => r.TokenHash);
        }



    }
}
