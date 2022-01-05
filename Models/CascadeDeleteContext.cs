using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DynamicDbContextWeb.Models;
public partial class CascadeDeleteContext : DbContext
{
    public CascadeDeleteContext(DbContextOptions<CascadeDeleteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChildTable> ChildTables
    {
        get; set;
    }
    public virtual DbSet<GrandChildTable> GrandChildTables
    {
        get; set;
    }
    public virtual DbSet<ParentTable> ParentTables
    {
        get; set;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AI");

        modelBuilder.Entity<ChildTable>(entity =>
        {
            entity.HasKey(e => e.IdChild);

            entity.ToTable("ChildTable");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdParentNavigation)
                .WithMany(p => p.ChildTables)
                .HasForeignKey(d => d.IdParent)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_ChildTable_ParentTable");
        });

        modelBuilder.Entity<GrandChildTable>(entity =>
        {
            entity.HasKey(e => e.IdGrandChild);

            entity.ToTable("GrandChildTable");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdChildNavigation)
                .WithMany(p => p.GrandChildTables)
                .HasForeignKey(d => d.IdChild)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_GrandChildTable_ChildTable");
        });

        modelBuilder.Entity<ParentTable>(entity =>
        {
            entity.HasKey(e => e.IdParent);

            entity.ToTable("ParentTable");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        foreach (var et in modelBuilder.Model.GetEntityTypes())
        {
            var entity = modelBuilder.Entity(et.ClrType);
            entity.Ignore(nameof(IDeletedEntity.IsDeleted));
            entity.Ignore(nameof(IAddedEntity.IsAdded));
            entity.Ignore(nameof(IModifiedEntity.IsModified));
        }
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
