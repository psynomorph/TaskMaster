using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task AddProjectMemberAsync(Project project, Employee employee)
        {
            project.Members.Add(employee);
            await _db.SaveChangesAsync();
        }

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
        public async Task<int> GetProjectCountAsync(FilteringModel filtering = null)
        {
            if (filtering is { })
            {
                return await AddFiltering(_db.Projects, filtering).CountAsync();
            }
            return await _db.Projects.CountAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            return await _db.Projects
                .Include(project => project.Leader)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Project>> GetProjectsAsync(SortState? sort = null, PageInfo pageInfo = null, FilteringModel filtering = null)
        {
            IQueryable<Project> query = _db.Projects
                .Include(project => project.Leader);

            if (filtering is { })
            {
                query = AddFiltering(query, filtering);
            }

            if (sort is { })
            {
                query = AddSorting(query, sort.Value);
            }

            if (pageInfo is { })
            {
                query = query
                    .Skip((pageInfo.PageNumber - 1) * pageInfo.ElementsOnPage)
                    .Take(pageInfo.ElementsOnPage);
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveProjectMemberAsync(Project project, Employee employee)
        {
            if (project.LeaderId == employee.Id)
            {
                throw new Exception("Project leader can not be removed from members list!"); // TODO: create custom exception.
            }

            project.Members.Remove(employee);
            await _db.SaveChangesAsync();
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

        /// <inheritdoc/>
        public async Task SetLeader(Project project, Employee employee)
        {
            bool leaderIsProjectMember = employee.Projects.Any(p => p.Id == project.Id);
            if (!leaderIsProjectMember)
            {
                employee.Projects.Add(project);
                _db.Employees.Update(employee);
            }

            project.Leader = employee;
            project.LeaderId = employee.Id;

            _db.Projects.Update(project);

            await _db.SaveChangesAsync();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Adds filters from <paramref name="filtering"/> to <paramref name="query"/>.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <param name="filtering">Params of filter.</param>
        /// <returns>Modified query.</returns>
        private static IQueryable<Project> AddFiltering(IQueryable<Project> query, FilteringModel filtering)
        {
            if (filtering.MinPriority is int minPriority)
            {
                query = query.Where(project => project.Priority >= minPriority);
            }

            if (filtering.MaxPriority is int maxPriority)
            {
                query = query.Where(project => project.Priority <= maxPriority);
            }

            if (filtering.MinBeginningDate is DateTime minBeginningDate)
            {
                query = query.Where(project => project.BegginingDate >= minBeginningDate);
            }

            if (filtering.MaxBeginningDate is DateTime maxBeginningDate)
            {
                query = query.Where(project => project.BegginingDate <= maxBeginningDate);
            }

            if (filtering.MinCompletionDate is DateTime minCompletionDate)
            {
                query = query.Where(project => project.CompletionDate >= minCompletionDate);
            }

            if (filtering.MaxCompletionDate is DateTime maxCompletiongDate)
            {
                query = query.Where(project => project.CompletionDate <= maxCompletiongDate);
            }

            return query;
        }

        /// <summary>
        /// Adds new project to database.
        /// </summary>
        /// <param name="project">Project.</param>
        private async Task AddProjectAsync(Project project)
        {
            var employee = await _db.Employees.FindAsync(project.LeaderId);
            project.Members = new List<Employee>();
            project.Members.Add(employee);
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Add ordering to query.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <param name="sort">Sorting params.</param>
        /// <returns>Query with ordering.</returns>
        private IQueryable<Project> AddSorting(IQueryable<Project> query, SortState sort) 
            => sort switch
            {
                SortState.NameAsc => query.OrderBy(project => project.Name),
                SortState.NameDesc => query.OrderByDescending(project => project.Name),

                SortState.PriorityAsc => query.OrderBy(project => project.Priority),
                SortState.PriorityDesc => query.OrderByDescending(project => project.Priority),

                SortState.BeginningDateAsc => query.OrderBy(project => project.BegginingDate),
                SortState.BeginningDateDesc => query.OrderByDescending(project => project.BegginingDate),

                SortState.CompletionDateAsc => query.OrderBy(project => project.CompletionDate),
                SortState.CompletionDateDesc => query.OrderByDescending(project => project.CompletionDate),

                _ => throw new ArgumentException($"Unknown sorting state {sort}")
            };

        /// <summary>
        /// Updates project in database.
        /// </summary>
        /// <param name="project">Project model with new data.</param>
        private async Task UpdateProjectAsync(Project project)
        {
            var leader = await _db
                .Employees
                .AsNoTracking()
                .Include(employee => employee.Projects)
                .FirstAsync(employee => employee.Id == project.LeaderId);

            bool leaderIsProjectMember = leader.Projects.Any(p => p.Id == project.Id); 

            if (!leaderIsProjectMember)
            {
                leader.Projects.Add(project);
                _db.Employees.Update(leader);
            }

            _db.Projects.Update(project);
            await _db.SaveChangesAsync();
        }

        #endregion Private Methods

    }
}
