using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Data_capture.Models
{
    public partial class DataCaptureContext : DbContext
    {
        public DataCaptureContext()
        {
        }

        public DataCaptureContext(DbContextOptions<DataCaptureContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<EntryCategory> EntryCategories { get; set; }
        public virtual DbSet<Measurement> Measurements { get; set; }
        public virtual DbSet<MeasurementMetaDatum> MeasurementMetaData { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=19013083.database.windows.net;Initial Catalog=WeatherCaptureDb;User ID=Sandile19013083;Password=Peenutbutter101;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId })
                    .HasName("PK_dbo.AspNetUserLogins");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_dbo.AspNetUserRoles");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<EntryCategory>(entity =>
            {
                entity.HasKey(e => e.EcId)
                    .HasName("PK__EntryCat__39330DC1FFF5E7DC");

                entity.Property(e => e.EcName).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EntryCategories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EntryCate__userI__7D439ABD");
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.HasKey(e => e.MId)
                    .HasName("PK__Measurem__DF513ED4FC5D9D53");

                entity.HasOne(d => d.Ec)
                    .WithMany(p => p.Measurements)
                    .HasForeignKey(d => d.EcId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Measuremen__ecId__114A936A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Measurements)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Measureme__userI__10566F31");
            });

            modelBuilder.Entity<MeasurementMetaDatum>(entity =>
            {
                entity.HasKey(e => e.MmdId)
                    .HasName("PK__Measurem__C6731F5D1F92B681");

                entity.Property(e => e.MmdTimeStamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.MIdNavigation)
                    .WithMany(p => p.MeasurementMetaData)
                    .HasForeignKey(d => d.MId)
                    .HasConstraintName("FK__Measurement__mId__14270015");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
