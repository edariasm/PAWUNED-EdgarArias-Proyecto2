using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PAWUNED_EdgarArias_Proyecto2.Models;

public partial class Proyecto2Context : DbContext
{
    public Proyecto2Context()
    {
    }

    public Proyecto2Context(DbContextOptions<Proyecto2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<ObrasArte> ObrasArtes { get; set; }

    public virtual DbSet<Oferta> Ofertas { get; set; }

    public virtual DbSet<Subasta> Subastas { get; set; }

    public virtual DbSet<Transaccione> Transacciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-2K6HMRM\\SQLEXPRESS; Database=Proyecto2; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ObrasArte>(entity =>
        {
            entity.HasKey(e => e.IdObra);

            entity.ToTable("ObrasArte");

            entity.Property(e => e.IdObra).HasColumnName("Id_obra");
            entity.Property(e => e.CategoriaObra)
                .HasMaxLength(50)
                .HasColumnName("Categoria_obra");
            entity.Property(e => e.DescripcionObra).HasColumnName("Descripcion_obra");
            entity.Property(e => e.DimensionesObra).HasColumnName("Dimensiones_obra");
            entity.Property(e => e.DuracionSubasta).HasColumnName("Duracion_subasta");
            entity.Property(e => e.FechaInicioSubasta)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_inicio_subasta");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.ImagenesObra).HasColumnName("Imagenes_obra");
            entity.Property(e => e.PrecioInicial)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Precio_inicial");
            entity.Property(e => e.TituloObra).HasColumnName("Titulo_obra");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ObrasArtes)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ObrasArte_Usuarios");
        });

        modelBuilder.Entity<Oferta>(entity =>
        {
            entity.HasKey(e => e.IdOferta);

            entity.Property(e => e.IdOferta).HasColumnName("Id_oferta");
            entity.Property(e => e.IdSubasta).HasColumnName("Id_subasta");
            entity.Property(e => e.IdUsuarioComprador).HasColumnName("Id_usuario_comprador");
            entity.Property(e => e.MontoOferta)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Monto_oferta");

            entity.HasOne(d => d.IdSubastaNavigation).WithMany(p => p.Oferta)
                .HasForeignKey(d => d.IdSubasta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ofertas_Subastas");

            entity.HasOne(d => d.IdUsuarioCompradorNavigation).WithMany(p => p.Oferta)
                .HasForeignKey(d => d.IdUsuarioComprador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ofertas_Usuarios");
        });

        modelBuilder.Entity<Subasta>(entity =>
        {
            entity.HasKey(e => e.IdSubasta);

            entity.Property(e => e.IdSubasta).HasColumnName("Id_subasta");
            entity.Property(e => e.FechaHoraCierre)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_hora_cierre");
            entity.Property(e => e.IdObra).HasColumnName("Id_obra");
            entity.Property(e => e.IdUsuarioGanador).HasColumnName("Id_usuario_ganador");
            entity.Property(e => e.PrecioActual)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Precio_actual");

            entity.HasOne(d => d.IdUsuarioGanadorNavigation).WithMany(p => p.Subasta)
                .HasForeignKey(d => d.IdUsuarioGanador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subastas_Usuarios");
        });

        modelBuilder.Entity<Transaccione>(entity =>
        {
            entity.HasKey(e => e.IdTransaccion);

            entity.Property(e => e.IdTransaccion).HasColumnName("Id_transaccion");
            entity.Property(e => e.FechaTransaccion).HasColumnName("Fecha_transaccion");
            entity.Property(e => e.IdObra).HasColumnName("Id_obra");
            entity.Property(e => e.IdUsuarioArtista).HasColumnName("Id_usuario_artista");
            entity.Property(e => e.IdUsuarioComprador).HasColumnName("Id_usuario_comprador");
            entity.Property(e => e.MontoTransaccion)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Monto_transaccion");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdObra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transacciones_ObrasArte");

            entity.HasOne(d => d.IdUsuarioCompradorNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdUsuarioComprador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transacciones_Usuarios");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
            entity.Property(e => e.BiografiaArtista).HasColumnName("Biografia_artista");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.EnlacePaginaArtista).HasColumnName("Enlace_pagina_artista");
            entity.Property(e => e.FotoUsuario).HasColumnName("Foto_usuario");
            entity.Property(e => e.NombreArtista).HasColumnName("Nombre_artista");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .HasColumnName("Nombre_usuario");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.TipoUsuario)
                .HasMaxLength(50)
                .HasColumnName("Tipo_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
