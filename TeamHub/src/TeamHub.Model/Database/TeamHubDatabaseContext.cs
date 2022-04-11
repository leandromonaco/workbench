using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TeamHub.Model.Database
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

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Leave> Leaves { get; set; }
        public virtual DbSet<LeaveType> LeaveTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<PublicHoliday> PublicHolidays { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Timebox> Timeboxes { get; set; }
        public virtual DbSet<TimeboxType> TimeboxTypes { get; set; }
        public virtual DbSet<Transfer> Transfers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-G6NANMQJ\\SQLEXPRESS;Initial Catalog=TeamHub.Database;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Employee_Location");

                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("FK_Employee_Employee");

                entity.HasOne(d => d.Specialization)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.SpecializationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Developer_Specialization");
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Leave)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Leave_Employee");

                entity.HasOne(d => d.LeaveType)
                    .WithMany(p => p.Leave)
                    .HasForeignKey(d => d.LeaveTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdminTime_AdminTimeCategory");
            });

            modelBuilder.Entity<LeaveType>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TimeZone)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PublicHoliday>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.PublicHoliday)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_PublicHoliday_Location");
            });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.From).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.To).HasColumnType("date");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Timebox>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Sprint)
                    .WithMany(p => p.Timebox)
                    .HasForeignKey(d => d.SprintId)
                    .HasConstraintName("FK_Timebox_Sprint");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Timebox)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timebox_DevelopmentTeam");

                entity.HasOne(d => d.TimeboxType)
                    .WithMany(p => p.Timebox)
                    .HasForeignKey(d => d.TimeboxTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timebox_TimeboxCategory");
            });

            modelBuilder.Entity<TimeboxType>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Transfer)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamChanges_Developer");

                entity.HasOne(d => d.FromTeam)
                    .WithMany(p => p.TransferFromTeam)
                    .HasForeignKey(d => d.FromTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamChanges_Team");

                entity.HasOne(d => d.ToTeam)
                    .WithMany(p => p.TransferToTeam)
                    .HasForeignKey(d => d.ToTeamId)
                    .HasConstraintName("FK_Transfer_Team");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
