using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TeamHub.API.Database
{
    public partial class TeamHubDatabaseContext : DbContext
    {
        public TeamHubDatabaseContext()
        {
        }

        public TeamHubDatabaseContext(DbContextOptions<TeamHubDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Leave> Leaves { get; set; } = null!;
        public virtual DbSet<LeaveType> LeaveTypes { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<PublicHoliday> PublicHolidays { get; set; } = null!;
        public virtual DbSet<Specialization> Specializations { get; set; } = null!;
        public virtual DbSet<Sprint> Sprints { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<Timebox> Timeboxes { get; set; } = null!;
        public virtual DbSet<TimeboxType> TimeboxTypes { get; set; } = null!;
        public virtual DbSet<Transfer> Transfers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-G6NANMQJ\\SQLEXPRESS;Initial Catalog=TeamHub.Database;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Employee_Location");

                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("FK_Employee_Employee");

                entity.HasOne(d => d.Specialization)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.SpecializationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Developer_Specialization");
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.ToTable("Leave");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Leaves)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Leave_Employee");

                entity.HasOne(d => d.LeaveType)
                    .WithMany(p => p.Leaves)
                    .HasForeignKey(d => d.LeaveTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdminTime_AdminTimeCategory");
            });

            modelBuilder.Entity<LeaveType>(entity =>
            {
                entity.ToTable("LeaveType");

                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.TimeZone).HasMaxLength(50);
            });

            modelBuilder.Entity<PublicHoliday>(entity =>
            {
                entity.ToTable("PublicHoliday");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.PublicHolidays)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_PublicHoliday_Location");
            });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.ToTable("Specialization");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.ShortDescription).HasMaxLength(10);
            });

            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.ToTable("Sprint");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.From).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.To).HasColumnType("date");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Timebox>(entity =>
            {
                entity.ToTable("Timebox");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Sprint)
                    .WithMany(p => p.Timeboxes)
                    .HasForeignKey(d => d.SprintId)
                    .HasConstraintName("FK_Timebox_Sprint");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Timeboxes)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timebox_DevelopmentTeam");

                entity.HasOne(d => d.TimeboxType)
                    .WithMany(p => p.Timeboxes)
                    .HasForeignKey(d => d.TimeboxTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timebox_TimeboxCategory");
            });

            modelBuilder.Entity<TimeboxType>(entity =>
            {
                entity.ToTable("TimeboxType");

                entity.Property(e => e.Description).HasMaxLength(50);
            });

            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.ToTable("Transfer");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Transfers)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamChanges_Developer");

                entity.HasOne(d => d.FromTeam)
                    .WithMany(p => p.TransferFromTeams)
                    .HasForeignKey(d => d.FromTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamChanges_Team");

                entity.HasOne(d => d.ToTeam)
                    .WithMany(p => p.TransferToTeams)
                    .HasForeignKey(d => d.ToTeamId)
                    .HasConstraintName("FK_Transfer_Team");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
