using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdviceAssignement.DAL.Entities;

public partial class ElevatorsManagementDbContext : DbContext
{
    public ElevatorsManagementDbContext()
    {
    }

    public ElevatorsManagementDbContext(DbContextOptions<ElevatorsManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Elevator> Elevators { get; set; }

    public virtual DbSet<ElevatorCall> ElevatorCalls { get; set; }

    public virtual DbSet<ElevatorCallAssignment> ElevatorCallAssignments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-CPV9KC4\\SQLEXPRESS;Database=ElevatorsManagementDB;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Building__3214EC07E7DEAF05");

            entity.Property(e => e.Name).HasMaxLength(25);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Buildings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Buildings_Users");
        });

        modelBuilder.Entity<Elevator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Elevator__3214EC07F30BDE81");

            entity.HasIndex(e => e.BuildingId, "UQ__Elevator__5463CDE55C1519E0").IsUnique();

            entity.Property(e => e.BuildingId).HasColumnName("BuildingID");

            entity.HasOne(d => d.Building).WithOne(p => p.Elevator)
                .HasForeignKey<Elevator>(d => d.BuildingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Elevators_Buildings");
        });

        modelBuilder.Entity<ElevatorCall>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Elevator__3214EC0757C0BB75");

            entity.Property(e => e.BuildingId).HasColumnName("BuildingID");
            entity.Property(e => e.CallTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Building).WithMany(p => p.ElevatorCalls)
                .HasForeignKey(d => d.BuildingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ElevatorCalls_Buildings");
        });

        modelBuilder.Entity<ElevatorCallAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Elevator__3214EC076298E5BB");

            entity.ToTable("ElevatorCallAssignment");

            entity.Property(e => e.AssignmentTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Elevator).WithMany(p => p.ElevatorCallAssignments)
                .HasForeignKey(d => d.ElevatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ElevatorCallAssignment_Elevators");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC073EA9D4E7");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534DDB2B081").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
