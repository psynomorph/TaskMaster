using System.Collections.Generic;
using System.Threading.Tasks;
using TaskMaster.Models;

namespace TaskMaster.Services
{
    /// <summary>
    /// Projects repository.
    /// </summary>
    public interface IProjectRepository
    {
        /// <summary>
        /// Returns all projects in repository.
        /// </summary>
        /// <returns>Collection of projects.</returns>
        Task<IEnumerable<Project>> GetProjectsAsync();

        /// <summary>
        /// Returns project by it id.
        /// </summary>
        /// <param name="id">Project identifier.</param>
        /// <returns>Project or <see langword="null"/></returns>
        Task<Project> GetProjectAsync(int id);

        /// <summary>
        /// Saves project.
        /// </summary>
        /// <param name="project">Project.</param>
        Task SaveProjectAsync(Project project);

        /// <summary>
        /// Deletes project.
        /// </summary>
        /// <param name="project">Project to delete.</param>
        Task DeleteProjectAsync(Project project);
    }
}
