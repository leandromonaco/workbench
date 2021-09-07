using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
namespace TeamAlignment.Core.Model.Database
{
    public partial class TeamAlignmentContext : DbContext
    {
        IConfiguration _configuration;
        public TeamAlignmentContext()
        {
        }

        public TeamAlignmentContext(DbContextOptions<TeamAlignmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CarryOverPoints> CarryOverPoints { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<KeyResult> KeyResults { get; set; }
        public virtual DbSet<Leave> Leaves { get; set; }
        public virtual DbSet<LeaveType> LeaveTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Milestone> Milestones { get; set; }
        public virtual DbSet<Objective> Objectives { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<PublicHoliday> PublicHolidays { get; set; }
        public virtual DbSet<PublicHolidayLocation> PublicHolidaysLocations { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<QuestionnairePeriod> QuestionnairePeriods { get; set; }
        public virtual DbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
        public virtual DbSet<QuestionnaireSection> QuestionnaireSections { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Snapshot> Snapshots { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamChange> TeamChanges { get; set; }
        public virtual DbSet<Timebox> Timeboxes { get; set; }
        public virtual DbSet<TimeboxType> TimeboxTypes { get; set; }
        public virtual DbSet<Timezone> Timezones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                _configuration = builder.Build();
                //See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("TeamAlignmentDatabase"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarryOverPoints>(entity =>
            {
                entity.ToTable("CarryOverPoints", "planning");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CarryOverPoints1).HasColumnName("CarryOverPoints");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee", "company");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LoginUser)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("FK_Employee_Employee");
            });

            modelBuilder.Entity<KeyResult>(entity =>
            {
                entity.ToTable("KeyResult", "okrs");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.ToTable("Leave", "planning");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Leaves)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdminTime_AdminTimeCategory");

                entity.HasOne(d => d.TeamMember)
                    .WithMany(p => p.Leaves)
                    .HasForeignKey(d => d.TeamMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Leave_Employee");
            });

            modelBuilder.Entity<LeaveType>(entity =>
            {
                entity.ToTable("LeaveType", "category");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "company");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.TimeZone)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.TimeZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Location_Timezone");
            });

            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.ToTable("Milestone", "calendar");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Milestones)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Milestone_Teams");
            });

            modelBuilder.Entity<Objective>(entity =>
            {
                entity.ToTable("Objective", "okrs");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "company");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Configuration)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ConfigurationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Configuration");
            });

            modelBuilder.Entity<PublicHoliday>(entity =>
            {
                entity.ToTable("PublicHoliday", "calendar");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<PublicHolidayLocation>(entity =>
            {
                entity.HasKey(e => new { e.LocationId, e.PublicHolidayId })
                    .HasName("PK_PublicHolidayLocationCity");

                entity.ToTable("PublicHolidayLocation", "calendar");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.PublicHolidaysLocations)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PublicHolidayLocationLocation_Locations");

                entity.HasOne(d => d.PublicHoliday)
                    .WithMany(p => p.PublicHolidaysLocations)
                    .HasForeignKey(d => d.PublicHolidayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PublicHolidayLocationCity_PublicHolidayLocation");
            });

            modelBuilder.Entity<Questionnaire>(entity =>
            {
                entity.ToTable("Questionnaire", "survey");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description).IsRequired();

                entity.Ignore(e => e.QuestionnairePeriod);
            });

            modelBuilder.Entity<QuestionnairePeriod>(entity =>
            {
                entity.ToTable("QuestionnairePeriod", "survey");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.From).HasColumnType("datetime");

                entity.Property(e => e.To).HasColumnType("datetime");
            });

            modelBuilder.Entity<QuestionnaireQuestion>(entity =>
            {
                entity.ToTable("QuestionnaireQuestion", "survey");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.SectionId).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.QuestionnaireQuestions)
                    .HasForeignKey(d => d.SectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionnaireQuestion_QuestionnaireSection");

                entity.Ignore(e => e.Score);
            });

            modelBuilder.Entity<QuestionnaireSection>(entity =>
            {
                entity.ToTable("QuestionnaireSection", "survey");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.QuestionnaireId).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Questionnaire)
                    .WithMany(p => p.QuestionnaireSections)
                    .HasForeignKey(d => d.QuestionnaireId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionnaireSection_Questionnaire");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting", "company");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Snapshot>(entity =>
            {
                entity.ToTable("Snapshot", "company");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Snapshots)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Snapshot_Employee");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Snapshots)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Snapshot_Product");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Snapshots)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_Snapshot_Team");
            });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.ToTable("Specialization", "category");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team", "company");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ReleasePlanningCutOff).HasColumnType("datetime");

                entity.Property(e => e.SprintPlanningCutOff).HasColumnType("datetime");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DevelopmentTeam_Locations");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DevelopmentTeam_Product");
            });

            modelBuilder.Entity<TeamChange>(entity =>
            {
                entity.ToTable("TeamChange", "planning");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.FirstDay).HasColumnType("date");

                entity.Property(e => e.LastDay).HasColumnType("date");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamChanges)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamChanges_Team");

                entity.HasOne(d => d.TeamMember)
                    .WithMany(p => p.TeamChanges)
                    .HasForeignKey(d => d.TeamMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamChanges_Developer");
            });

            modelBuilder.Entity<Timebox>(entity =>
            {
                entity.ToTable("Timebox", "planning");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.WorkItemId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Timeboxes)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timebox_DevelopmentTeam");

                entity.HasOne(d => d.TimeboxCategory)
                    .WithMany(p => p.Timeboxes)
                    .HasForeignKey(d => d.TimeboxCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Timebox_TimeboxCategory");
            });

            modelBuilder.Entity<TimeboxType>(entity =>
            {
                entity.ToTable("TimeboxType", "category");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Timezone>(entity =>
            {
                entity.ToTable("Timezone", "calendar");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
