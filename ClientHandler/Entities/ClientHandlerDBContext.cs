using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClientHandler.Entities
{
    public partial class ClientHandlerDBContext : DbContext
    {
        public ClientHandlerDBContext()
        {
        }

        public ClientHandlerDBContext(DbContextOptions<ClientHandlerDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Governorate> Governorates { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Village> Villages { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityName)
                    .HasMaxLength(50)
                    .HasColumnName("city_name");

                entity.Property(e => e.GovernorateId).HasColumnName("governorate_id");

                entity.HasOne(d => d.Governorate)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.GovernorateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_governorate");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.NationalId)
                    .HasName("PK__Clients__9560E95CB85B0D67");

                entity.Property(e => e.NationalId)
                    .HasMaxLength(14)
                    .HasColumnName("national_id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Salary)
                    .HasColumnType("decimal(19, 4)")
                    .HasColumnName("salary");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.VillageId).HasColumnName("village_id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__Clients__city_id__571DF1D5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user");

                entity.HasOne(d => d.Village)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.VillageId)
                    .HasConstraintName("FK__Clients__village__5812160E");
            });

            modelBuilder.Entity<Governorate>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GovernorateName)
                    .HasMaxLength(50)
                    .HasColumnName("governorate_name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(25)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.HashedPassword).HasColumnName("hashed_password");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(256)
                    .HasColumnName("username");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_roles");
            });

            modelBuilder.Entity<Village>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.VillageName)
                    .HasMaxLength(50)
                    .HasColumnName("village_name");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Villages)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_city");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
