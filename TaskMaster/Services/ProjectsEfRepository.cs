﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Models;

namespace TaskMaster.Services
{
    /// <summary>
    /// Projects repository.
    /// </summary>
    public class ProjectsEfRepository : IProjectRepository
    {

        #region Private Fields

        /// <summary>
        /// Database context.
        /// </summary>
        private readonly DatabaseContext _db;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates new projects repository.
        /// </summary>
        /// <param name="db">Database context.</param>
        public ProjectsEfRepository(DatabaseContext db)
        {
            _db = db;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <inheritdoc/>
        public async Task DeleteProjectAsync(Project project)
        {
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Project> GetProjectAsync(int id)
        {
            return await _db.Projects
                .Where(project => project.Id == id)
                .Include(project => project.Leader)
                .Include(project => project.Members)
                .SingleOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await _db.Projects
                .Include(project => project.Leader)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task SaveProjectAsync(Project project)
        {
            if (project.IsNew())
            {
                await AddProjectAsync(project);
            }
            else
            {
                await UpdateProjectAsync(project);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Adds new project to database.
        /// </summary>
        /// <param name="project">Project.</param>
        private async Task AddProjectAsync(Project project)
        {
            var employee = await _db.Employees.FindAsync(project.LeaderId);
            project.Members.Add(employee);
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Updates project in database.
        /// </summary>
        /// <param name="project">Project model with new data.</param>
        private async Task UpdateProjectAsync(Project project)
        {
            _db.Projects.Update(project);
            await _db.SaveChangesAsync();
        }

        #endregion Private Methods

    }
}