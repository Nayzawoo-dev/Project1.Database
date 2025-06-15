using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project5Database.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAttendend> TblAttendends { get; set; }

    public virtual DbSet<TblAttendendHistory> TblAttendendHistories { get; set; }

    public virtual DbSet<TblStudent> TblStudents { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAttendend>(entity =>
        {
            entity.HasKey(e => e.AttendendId);

            entity.ToTable("Tbl_Attendend");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RollNo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComputedColumnSql("('CST-'+right('000'+CONVERT([varchar],[AttendendId]),(3)))", false);
        });

        modelBuilder.Entity<TblAttendendHistory>(entity =>
        {
            entity.HasKey(e => e.AttendendHistoryId);

            entity.ToTable("Tbl_AttendendHistory");

            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RollNo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblStudent>(entity =>
        {
            entity.HasKey(e => e.StudentId);

            entity.ToTable("Tbl_Student");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RollNo)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComputedColumnSql("('CST-'+right('000'+CONVERT([varchar],[StudentId]),(3)))", false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
