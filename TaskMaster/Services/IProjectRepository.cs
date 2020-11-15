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

        /// <summary>
        /// Add <paramref name="employee"/> to list of <paramref name="project"/> members.
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="employee">Employee to add to project.</param>
        Task AddProjectMemberAsync(Project project, Employee employee);

        /// <summary>
        /// Remove <paramref name="employee"/> from list of project members.
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="employee">Employee for remove.</param>
        Task RemoveProjectMemberAsync(Project project, Employee employee);

        /// <summary>
        /// Makes <paramref name="employee"/> leader of <paramref name="project"/>.
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="employee">Employee.</param>
        /// <returns></returns>
        Task SetLeader(Project project, Employee employee);

        /// <summary>
        /// Returns all projects in repository sorted by rule defined in <paramref name="sort"/>.
        /// </summary>
        /// <param name="sort">Property and direction of sorting enum.</param>
        /// <param name="filtering">Params of items filtering.</param>
        /// <param name="pageInfo">Info about page</param>
        /// <returns>Collection of projects.</returns>
        Task<IEnumerable<Project>> GetProjectsAsync(SortState? sort = null, PageInfo pageInfo = null, FilteringModel filtering = null);

        /// <summary>
        /// Returns count of projects that match the filter.
        /// </summary>
        /// <param name="filtering">Params of filter.</param>
        /// <returns>Count of projects matched by filter.</returns>
        Task<int> GetProjectCountAsync(FilteringModel filtering);
    }
}
