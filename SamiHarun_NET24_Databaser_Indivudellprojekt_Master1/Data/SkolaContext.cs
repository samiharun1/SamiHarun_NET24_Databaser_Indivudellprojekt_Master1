using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SamiHarun_NET24_Databaser_Indivudellprojekt_Master1.Models;

namespace SamiHarun_NET24_Databaser_Indivudellprojekt_Master1.Data;

public partial class SkolaContext : DbContext
{
    public SkolaContext()
    {
    }

    public SkolaContext(DbContextOptions<SkolaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Betyg> Betygs { get; set; }

    public virtual DbSet<Kurser> Kursers { get; set; }

    public virtual DbSet<Personal> Personals { get; set; }

    public virtual DbSet<Studenter> Studenters { get; set; }

    public virtual DbSet<ElevInfo> ElevInfo { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Skola;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Betyg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Betyg__3214EC27A898C11A");

            entity.ToTable("Betyg");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Betyg1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Betyg");
            entity.Property(e => e.Datum).HasColumnType("datetime");
            entity.Property(e => e.KursId).HasColumnName("KursID");
            entity.Property(e => e.LarareId).HasColumnName("LarareID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Kurs).WithMany(p => p.Betygs)
                .HasForeignKey(d => d.KursId)
                .HasConstraintName("FK__Betyg__KursID__3D5E1FD2");

            entity.HasOne(d => d.Larare).WithMany(p => p.Betygs)
                .HasForeignKey(d => d.LarareId)
                .HasConstraintName("FK__Betyg__LarareID__3F466844");

            entity.HasOne(d => d.Student).WithMany(p => p.Betygs)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Betyg__StudentID__3E52440B");
        });

        modelBuilder.Entity<Kurser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kurser__3214EC2762A390C9");

            entity.ToTable("Kurser");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Kursnamn)
                .HasMaxLength(75)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Personal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Personal__3214EC2755855B3F");

            entity.ToTable("Personal");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Avdelning)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Befattning)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Lon).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Namn)
                .HasMaxLength(35)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Studenter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Studente__3214EC273C139594");

            entity.ToTable("Studenter");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Klass)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Namn)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Personnummer)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ElevInfo>().HasNoKey();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
