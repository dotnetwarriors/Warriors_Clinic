using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Chemist> Chemists { get; set; }

    public virtual DbSet<Drug> Drugs { get; set; }

    public virtual DbSet<DrugRequest> DrugRequests { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Physician> Physicians { get; set; }

    public virtual DbSet<PhysicianAdvice> PhysicianAdvices { get; set; }

    public virtual DbSet<PhysicianPrescription> PhysicianPrescriptions { get; set; }

    public virtual DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }

    public virtual DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }

    public virtual DbSet<Schedule> Schedule { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WarrirosClinic;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC204B77562");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments).HasConstraintName("FK__Appointme__Patie__5812160E");

            entity.HasOne(d => d.Physician).WithMany(p => p.Appointments).HasConstraintName("FK__Appointme__Physi__59063A47");
        });

        modelBuilder.Entity<DrugRequest>(entity =>
        {
            entity.HasKey(e => e.DrugRequestId).HasName("PK__DrugRequ__AEE9D630B65886DB");

            entity.Property(e => e.RequestDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Physician).WithMany(p => p.DrugRequests).HasConstraintName("FK__DrugReque__Physi__66603565");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9CDDBF30F9");

            entity.Property(e => e.SentDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers).HasConstraintName("FK__Messages__Receiv__72C60C4A");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders).HasConstraintName("FK__Messages__Sender__71D1E811");
        });

        modelBuilder.Entity<PhysicianAdvice>(entity =>
        {
            entity.HasKey(e => e.PhysicianAdviceId).HasName("PK__Physicia__82C625F0D3DA4DB3");

           
        });

        modelBuilder.Entity<PhysicianPrescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__Physicia__40130832B582FABF");

            entity.HasOne(d => d.Drug).WithMany(p => p.PhysicianPrescriptions).HasConstraintName("FK__Physician__DrugI__628FA481");

            entity.HasOne(d => d.PhysicianAdvice).WithMany(p => p.PhysicianPrescriptions).HasConstraintName("FK__Physician__Physi__619B8048");
        });

        modelBuilder.Entity<PurchaseOrderHeader>(entity =>
        {
            entity.HasKey(e => e.Poid).HasName("PK__Purchase__5F02A2D45CDE26B8");

            entity.Property(e => e.Podate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrderHeaders).HasConstraintName("FK__PurchaseO__Suppl__6A30C649");
        });

        modelBuilder.Entity<PurchaseOrderLine>(entity =>
        {
            entity.HasKey(e => e.PolineId).HasName("PK__Purchase__07B9D3621DB03421");

            entity.HasOne(d => d.Drug).WithMany(p => p.PurchaseOrderLines).HasConstraintName("FK__PurchaseO__DrugI__6E01572D");

            entity.HasOne(d => d.Po).WithMany(p => p.PurchaseOrderLines).HasConstraintName("FK__PurchaseOr__POId__6D0D32F4");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B495BE3D830");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Schedules).HasConstraintName("FK__Schedule__Appoin__5BE2A6F2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
