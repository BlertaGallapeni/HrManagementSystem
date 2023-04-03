using HRMS.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = HRMS.Models.Entities.Task;

namespace HRMS.DataAccess.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
            public DbSet<ApplicationUser> ApplicationUsers { get; set; }
            public DbSet<Company> Companies { get; set; }
            public DbSet<Department> Departments { get; set; }
            public DbSet<Employee> Employees { get; set; }
            public DbSet<Designation> Designations { get; set; }
            public DbSet<Client> Clients { get; set; }
            public DbSet<RateType> RateTypes { get; set; }
            public DbSet<Priority> Priorities  { get; set; }
            public DbSet<Status> Status  { get; set; }
            public DbSet<Team> Teams { get; set; }
            public DbSet<TeamMember> TeamMembers  { get; set; }
            public DbSet<Project> Projects  { get; set; }
            public DbSet<Task> Tasks  { get; set; }
            public DbSet<UserThumbnail> UserThumbnail  { get; set; }
            public DbSet<Leave> Leaves { get; set; }
            public DbSet<LeaveType> LeaveTypes { get; set; }
            public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamMember>()
                .HasKey(tm => new { tm.EmployeeId, tm.TeamId });

            modelBuilder.Entity<TeamMember>()
                .HasOne<Employee>(sc => sc.Employee)
                .WithMany(s => s.TeamMembers)
                .HasForeignKey(sc => sc.EmployeeId);


            modelBuilder.Entity<TeamMember>()
                .HasOne<Team>(sc => sc.Team)
                .WithMany(s => s.TeamMembers)
                .HasForeignKey(sc => sc.TeamId);

            modelBuilder.Entity<UserThumbnail>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Path).IsRequired();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.UserThumbnail)
                    .HasForeignKey<UserThumbnail>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserThumbnail_UserThumbnail");
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
