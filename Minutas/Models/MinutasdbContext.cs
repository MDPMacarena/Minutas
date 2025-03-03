using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Minutas.Models;

public partial class MinutasdbContext : DbContext
{
    public MinutasdbContext()
    {
    }

    public MinutasdbContext(DbContextOptions<MinutasdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departamento> Departamento { get; set; }

    public virtual DbSet<Minuta> Minuta { get; set; }

    public virtual DbSet<MinutaUsuario> MinutaUsuario { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    //Scaffold-DBContext "server=localhost;user=root;password=root;database=presidentes;port=3307" Pomelo.EntityFrameworkCore.MySql -o "Models" -nop -f
   
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "server=localhost;database=minutadb;user=root;password=root;port=3307";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("departamento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdDepSuperior).HasColumnName("idDepSuperior");
            entity.Property(e => e.IdJefe).HasColumnName("idJefe");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Minuta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("minuta");

            entity.HasIndex(e => e.IdDepartamento, "fk_MInutaDepartamento_idx");

            entity.HasIndex(e => e.IdCreador, "fk_MinutaUsuario_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contenidos)
                .HasColumnType("json")
                .HasColumnName("contenidos");
            entity.Property(e => e.Estado)
                .HasColumnType("enum('Pendiente','Completada','Cancelada')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion).HasColumnName("fechaCreacion");
            entity.Property(e => e.IdCreador).HasColumnName("idCreador");
            entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");

            entity.HasOne(d => d.IdCreadorNavigation).WithMany(p => p.Minuta)
                .HasForeignKey(d => d.IdCreador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MinutaUsuario");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Minuta)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MInutaDepartamento");
        });

        modelBuilder.Entity<MinutaUsuario>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdMinuta })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("minuta_usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.IdMinuta).HasColumnName("idMinuta");
            entity.Property(e => e.Firmado)
                .HasDefaultValueSql("'0'")
                .HasColumnName("firmado");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MinutaUsuario)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MinutaUsuario2");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Correo, "Correo").IsUnique();

            entity.HasIndex(e => e.NumEmpleado, "NumControl_Empleado").IsUnique();

            entity.HasIndex(e => e.IdDepartamento, "fk_UsuarioDepartamento_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("activo");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fechaNacimiento");
            entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.NumEmpleado)
                .HasMaxLength(50)
                .HasColumnName("numEmpleado");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UsuarioDepartamento");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
