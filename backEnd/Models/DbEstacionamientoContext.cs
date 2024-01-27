using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Models;

public partial class DbEstacionamientoContext : DbContext
{
    public DbEstacionamientoContext()
    {
    }

    public DbEstacionamientoContext(DbContextOptions<DbEstacionamientoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarType> CarTypes { get; set; }

    public virtual DbSet<PaymentFormat> PaymentFormats { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=localhost;Database=db_estacionamiento;User ID=postgres;Password=123456;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.IdBrand).HasName("brands_pkey");

            entity.ToTable("brands");

            entity.Property(e => e.IdBrand).HasColumnName("id_brand");
            entity.Property(e => e.BrandName)
                .HasColumnType("character varying")
                .HasColumnName("brand");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.IdCar).HasName("cars_pkey");

            entity.ToTable("cars");

            entity.Property(e => e.IdCar)
                .ValueGeneratedNever()
                .HasColumnName("id_car");
            entity.Property(e => e.AdmissionDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("admission_date");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.Brand).HasColumnName("brand");
            entity.Property(e => e.DischargeDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("discharge_date");
            entity.Property(e => e.Format).HasColumnName("format");
            entity.Property(e => e.Garage).HasColumnName("garage");
            entity.Property(e => e.Location)
                .HasColumnType("character varying")
                .HasColumnName("location");
            entity.Property(e => e.Patent)
                .HasColumnType("character varying")
                .HasColumnName("patent");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.BrandNavigation).WithMany(p => p.Cars)
                .HasForeignKey(d => d.Brand)
                .HasConstraintName("fk_brand");

            entity.HasOne(d => d.FormatNavigation).WithMany(p => p.Cars)
                .HasForeignKey(d => d.Format)
                .HasConstraintName("fk_format");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Cars)
                .HasForeignKey(d => d.Type)
                .HasConstraintName("fk_car_type");
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => e.IdCarType).HasName("car_type_pkey");

            entity.ToTable("car_type");

            entity.Property(e => e.IdCarType).HasColumnName("id_car_type");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
        });

        modelBuilder.Entity<PaymentFormat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_format_pkey");

            entity.ToTable("payment_format");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Format)
                .HasColumnType("character varying")
                .HasColumnName("format");
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Price_pkey");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('\"Price_id_seq\"'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.PriceName)
                .HasColumnType("money")
                .HasColumnName("price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUsers).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.IdUsers)
                .ValueGeneratedNever()
                .HasColumnName("id_users");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
