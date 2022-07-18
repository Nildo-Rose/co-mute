using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace co_mute_master.Models
{
    public partial class DbEntities : DbContext
    {
        public DbEntities()
        {
        }

        public DbEntities(DbContextOptions<DbEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<CarPool> CarPools { get; set; }
        public virtual DbSet<JoinLeaveOpp> JoinLeaveOpps { get; set; }
        public virtual DbSet<Register> Registers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CarPool>(entity =>
            {
                entity.ToTable("CarPool");

                entity.Property(e => e.AvailableSeats).HasMaxLength(250);

                entity.Property(e => e.Destination).HasMaxLength(250);

                entity.Property(e => e.Origins).HasMaxLength(250);

                entity.Property(e => e.Owner).HasMaxLength(250);

                entity.Property(e => e.Rates).HasMaxLength(250);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CarPools)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_CarPool_Register");
            });

            modelBuilder.Entity<JoinLeaveOpp>(entity =>
            {
                entity.ToTable("JoinLeaveOpp");

                entity.Property(e => e.DateJoined).HasColumnType("date");

                entity.HasOne(d => d.CarOpp)
                    .WithMany(p => p.JoinLeaveOpps)
                    .HasForeignKey(d => d.CarOppId)
                    .HasConstraintName("FK_JoinLeaveOpp_CarPool");

                entity.HasOne(d => d.Reg)
                    .WithMany(p => p.JoinLeaveOpps)
                    .HasForeignKey(d => d.RegId)
                    .HasConstraintName("FK_JoinLeaveOpp_Register");
            });

            modelBuilder.Entity<Register>(entity =>
            {
                entity.ToTable("Register");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(10);

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("UserRole");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Register");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
