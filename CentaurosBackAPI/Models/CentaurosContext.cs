using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CentaurosBackAPI.Models;

public partial class CentaurosContext : DbContext
{
    public CentaurosContext()
    {
    }

    public CentaurosContext(DbContextOptions<CentaurosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ciudade> Ciudades { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<Diploma> Diplomas { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=CentaurosDBContext", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Ciudade>(entity =>
        {
            entity.HasKey(e => e.IdCiudad).HasName("PRIMARY");

            entity.ToTable("ciudades");

            entity.Property(e => e.IdCiudad).HasColumnName("idCiudad");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(45)
                .HasColumnName("ciudad");
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.IdCurso).HasName("PRIMARY");

            entity.ToTable("cursos");

            entity.Property(e => e.IdCurso).HasColumnName("idCurso");
            entity.Property(e => e.Curso1)
                .HasMaxLength(150)
                .HasColumnName("curso");
            entity.Property(e => e.HorasDuracion).HasColumnName("horasDuracion");
        });

        modelBuilder.Entity<Diploma>(entity =>
        {
            entity.HasKey(e => e.IdDiploma).HasName("PRIMARY");

            entity.ToTable("diplomas");

            entity.HasIndex(e => e.Ciudad, "fk_idCiudad_diplomas_idx");

            entity.HasIndex(e => e.IdCurso, "fk_idCurso_diplomas_idx");

            entity.HasIndex(e => e.IdEstudiante, "fk_idEstudiante_diplomas_idx");

            entity.Property(e => e.IdDiploma).HasColumnName("idDiploma");
            entity.Property(e => e.Ciudad).HasColumnName("ciudad");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdCurso).HasColumnName("idCurso");
            entity.Property(e => e.IdEstudiante)
                .HasMaxLength(10)
                .HasColumnName("idEstudiante");
            entity.Property(e => e.Nci).HasColumnName("NCI");
            entity.Property(e => e.Nro).HasColumnName("NRO");

            entity.HasOne(d => d.CiudadNavigation).WithMany(p => p.Diplomas)
                .HasForeignKey(d => d.Ciudad)
                .HasConstraintName("fk_idCiudad_diplomas");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Diplomas)
                .HasForeignKey(d => d.IdCurso)
                .HasConstraintName("fk_idCurso_diplomas");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Diplomas)
                .HasForeignKey(d => d.IdEstudiante)
                .HasConstraintName("fk_cedula_diplomas");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PRIMARY");

            entity.ToTable("estudiantes");

            entity.HasIndex(e => e.Correo, "correo_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdUsuario, "fk_idUsuario_estudiantes_idx");

            entity.Property(e => e.Cedula)
                .HasMaxLength(10)
                .HasColumnName("cedula");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .HasColumnName("apellidos");
            entity.Property(e => e.Correo).HasColumnName("correo");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .HasColumnName("nombres");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_idUsuario_estudiantes");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Usuario1, "usuario_UNIQUE").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(45)
                .HasColumnName("contraseña");
            entity.Property(e => e.Usuario1).HasColumnName("usuario");

            entity.HasMany(d => d.IdRols).WithMany(p => p.IdUsuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuariosRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRol")
                        .HasConstraintName("fk_idRol_usuariosRoles"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("IdUsuario")
                        .HasConstraintName("fk_idUsuario_usuariosRoles"),
                    j =>
                    {
                        j.HasKey("IdUsuario", "IdRol")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("usuarios_roles");
                        j.HasIndex(new[] { "IdRol" }, "fk_idRol_usuariosRoles_idx");
                        j.IndexerProperty<uint>("IdUsuario").HasColumnName("idUsuario");
                        j.IndexerProperty<uint>("IdRol").HasColumnName("idRol");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
