using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiGastos.Models
{
    public partial class bdGastosContext : DbContext
    {
        public bdGastosContext()
        {
        }

        public bdGastosContext(DbContextOptions<bdGastosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CategoriasGasto> CategoriasGastos { get; set; } = null!;
        public virtual DbSet<CategoriasIngreso> CategoriasIngresos { get; set; } = null!;
        public virtual DbSet<Gasto> Gastos { get; set; } = null!;
        public virtual DbSet<Ingreso> Ingresos { get; set; } = null!;
        public virtual DbSet<Pago> Pagos { get; set; } = null!;
        public virtual DbSet<Periocidad> Periocidads { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoriasGasto>(entity =>
            {
                entity.HasKey(e => e.IdCategoriasGasto)
                    .HasName("PK__Categori__6911A135D318FA8A");

                entity.ToTable("CategoriasGasto");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CategoriasIngreso>(entity =>
            {
                entity.HasKey(e => e.IdCategoriasIngreso)
                    .HasName("PK__Categori__3BBFECFEF6F2450E");

                entity.ToTable("CategoriasIngreso");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Gasto>(entity =>
            {
                entity.HasKey(e => e.IdGasto)
                    .HasName("PK__Gastos__C630244D779E858D");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.IdCategoriaGasto).HasColumnName("idCategoriaGasto");

                entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.objCategoriaGasto)
                    .WithMany(p => p.Gastos)
                    .HasForeignKey(d => d.IdCategoriaGasto)
                    .HasConstraintName("FK_Gastos_CategoriasGasto");
            });

            modelBuilder.Entity<Ingreso>(entity =>
            {
                entity.HasKey(e => e.IdIngreso)
                    .HasName("PK__Ingresos__901EF2E364F41450");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.IdCategoriaIngreso).HasColumnName("idCategoriaIngreso");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.objCategoriaIngreso)
                    .WithMany(p => p.Ingresos)
                    .HasForeignKey(d => d.IdCategoriaIngreso)
                    .HasConstraintName("FK_Ingresos_CategoriasIngreso");
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.HasKey(e => e.IdPago)
                    .HasName("PK__Pagos__FC851A3A48A99C39");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("date");
                entity.Property(e => e.FechaTentativa).HasColumnType("date");

                entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.IdMetodoPago).HasColumnName("IdMetodoPago");

                entity.HasOne(d => d.objCategoriaGasto)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdCategoriaGasto)
                    .HasConstraintName("FK_Pagos_CategoriasGasto");

                entity.HasOne(d => d.objPeriocidad)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.IdPeriocidad)
                    .HasConstraintName("FK_Pagos_IdPeriocidad");
            });

            modelBuilder.Entity<Periocidad>(entity =>
            {
                entity.HasKey(e => e.IdPeriocidad)
                    .HasName("PK__Periocid__6E10897B1A648F43");

                entity.ToTable("Periocidad");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__Roles__2A49584CEEE4B29C");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuarios__5B65BF970DE43AAB");

                entity.Property(e => e.Contrasena)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.objRoles)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK_Roles_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
