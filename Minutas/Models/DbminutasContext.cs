using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Minutas.Models;

public partial class DbminutasContext : DbContext
{
    public DbminutasContext()
    {
    }

    public DbminutasContext(DbContextOptions<DbminutasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departamento> Departamento { get; set; }

    public virtual DbSet<MinutaUsuario> MinutaUsuario { get; set; }

    public virtual DbSet<Minutas> Minutas { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=dbminutas", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("departamento");

            entity.HasIndex(e => e.IdDeptSuperior, "idDeptSuperior");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("activo");
            entity.Property(e => e.IdDeptSuperior).HasColumnName("idDeptSuperior");
            entity.Property(e => e.IdJefe).HasColumnName("idJefe");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdDeptSuperiorNavigation).WithMany(p => p.InverseIdDeptSuperiorNavigation)
                .HasForeignKey(d => d.IdDeptSuperior)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("departamento_ibfk_1");
        });

        modelBuilder.Entity<MinutaUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("minuta_usuario");

            entity.HasIndex(e => e.IdMinuta, "idMinuta");

            entity.HasIndex(e => e.IdUsuario, "idUsuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Firmado)
                .HasDefaultValueSql("'0'")
                .HasColumnName("firmado");
            entity.Property(e => e.IdMinuta).HasColumnName("idMinuta");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

            entity.HasOne(d => d.IdMinutaNavigation).WithMany(p => p.MinutaUsuario)
                .HasForeignKey(d => d.IdMinuta)
                .HasConstraintName("minuta_usuario_ibfk_2");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MinutaUsuario)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("minuta_usuario_ibfk_1");
        });

        modelBuilder.Entity<Minutas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("minutas");

            entity.HasIndex(e => e.IdCreador, "idCreador");

            entity.HasIndex(e => e.IdDepartamento, "idDepartamento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contenidos)
                .HasColumnType("json")
                .HasColumnName("contenidos");
            entity.Property(e => e.Estado)
                .HasColumnType("enum('PorFirmar','Firmada','Borrador')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion).HasColumnName("fechaCreacion");
            entity.Property(e => e.IdCreador).HasColumnName("idCreador");
            entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");

            entity.HasOne(d => d.IdCreadorNavigation).WithMany(p => p.Minutas)
                .HasForeignKey(d => d.IdCreador)
                .HasConstraintName("minutas_ibfk_1");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Minutas)
                .HasForeignKey(d => d.IdDepartamento)
                .HasConstraintName("minutas_ibfk_2");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "correo_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdDepartamento, "idDepartamento");

            entity.HasIndex(e => e.IdRol, "idRol");

            entity.HasIndex(e => e.NumEmpleado, "numEmpleado_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("activo");
            entity.Property(e => e.ContraseñaHash)
                .HasMaxLength(255)
                .HasColumnName("contraseña_hash");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fechaNacimiento");
            entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.NumEmpleado)
                .HasMaxLength(20)
                .HasColumnName("numEmpleado");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdDepartamento)
                .HasConstraintName("usuarios_ibfk_1");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("usuarios_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
