using Microsoft.EntityFrameworkCore;
using TaskMaster.Models;

namespace TaskMaster.Services
{
    /// <summary>
    /// Database context of application.
    /// </summary>
    public class DatabaseContext : DbContext
    {
        #region Public Constructors

        /// <summary>
        /// Creates database context and apply migrations if need.
        /// </summary>
        /// <param name="options">Database context options.</param>
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.Migrate();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Database set of employees.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Database set of projects.
        /// </summary>
        public DbSet<Project> Projects { get; set; }

        #endregion Public Properties

        #region Protected Methods

        /// <summary>
        /// Set up models relations params.
        /// </summary>
        /// <param name="modelBuilder">Model builder.</param>
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

        #endregion Protected Methods
    }
}
