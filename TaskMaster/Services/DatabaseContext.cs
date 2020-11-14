using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Models;

namespace TaskMaster.Services
{
    /// <summary>
    /// Database context of application.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(projectModelBuilder =>
            {
                projectModelBuilder
                    .HasMany(project => project.Members)
                    .WithMany(member => member.Projects);

                projectModelBuilder
                    .HasOne(project => project.Leader)
                    .WithMany(leader => leader.ProjectsWithLeadership)
                    .HasForeignKey(project => project.LeaderId);
            });
        }
    }
}
