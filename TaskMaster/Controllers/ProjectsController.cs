using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskMaster.Models;
using TaskMaster.Services;

using static TaskMaster.Helpers.RouteNames;

namespace TaskMaster.Controllers
{
    /// <summary>
    /// Projects controller.
    /// </summary>
    public class ProjectsController : Controller
    {
        #region Private Fields

        /// <summary>
        /// Employee repository.
        /// </summary>
        private readonly IEmployeeRepository _personsRepository;

        /// <summary>
        /// Projects repository.
        /// </summary>
        private readonly IProjectRepository _projectsRepository;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates projects controller.
        /// </summary>
        /// <param name="projectsRepository">Projects repository.</param>
        /// <param name="personsRepository">Persons repository.</param>
        /// <param name="session">Session storage.</param>
        public ProjectsController(IProjectRepository projectsRepository, IEmployeeRepository personsRepository)
        {
            _projectsRepository = projectsRepository;
            _personsRepository = personsRepository;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Returns project creation page.
        /// </summary>
        [Route("project/create", Name = CreateProjectPageRouteName)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var project = new Project();
            var persons = await _personsRepository.GetEmployeesAsync();
            ViewBag.Employees = persons;
            return View("Edit", project);
        }

        /// <summary>
        /// Create new project and redirects to it page.
        /// If model is not valid returns project creation view to continue editing.
        /// </summary>
        /// <param name="project">Project model.</param>
        [Route("project/create", Name = CreateProjectEndpointRouteName)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            if (!ModelState.IsValid)
            {
                var persons = await _personsRepository.GetEmployeesAsync();
                ViewBag.Employees = persons;

                return View("Edit", project);
            }

            await _projectsRepository.SaveProjectAsync(project);
            return RedirectToRoute(ProjectPageRouteName, new { id = project.Id });
        }

        /// <summary>
        /// Deletes project with <paramref name="id"/> identifier.
        /// </summary>
        /// <param name="id">Identifier of project to remove.</param>
        [Route("project/{id}/delete", Name = DeleteProjectEndpointRouteName)]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectsRepository.GetProjectAsync(id);
            if (project is null)
            {
                return NotFound();
            }

            await _projectsRepository.DeleteProjectAsync(project);

            return RedirectToRoute(ProjectsListPageRouteName);
        }

        /// <summary>
        /// Returns project editing page.
        /// </summary>
        /// <param name="id">Id of project to edit.</param>
        /// <returns></returns>
        [Route("project/{id}/edit", Name = EditProjectPageRouteName)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectsRepository.GetProjectAsync(id);
            if (project is null)
            {
                return NotFound();
            }

            var persons = await _personsRepository.GetEmployeesAsync();
            ViewBag.Employees = persons;

            return View(project);
        }

        /// <summary>
        /// Updates project in repository and redirects to project page.
        /// If project model is not valid returns page to continue editing.
        /// </summary>
        /// <param name="id">Id of project.</param>
        /// <param name="project">Model with new project data.</param>
        [Route("project/{id}/edit", Name = EditProjectEndpointRouteName)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                var persons = await _personsRepository.GetEmployeesAsync();
                ViewBag.Employees = persons;

                return View("Edit", project);
            }

            await _projectsRepository.SaveProjectAsync(project);
            return RedirectToRoute(ProjectPageRouteName, new { id });
        }

        /// <summary>
        /// Returns page with list of projects.
        /// </summary>
        [Route("", Name = ProjectsListPageRouteName)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await _projectsRepository.GetProjectsAsync();
            return View(projects);
        }

        /// <summary>
        /// Returns page with project details.
        /// </summary>
        /// <param name="id">Id of project.</param>
        [Route("project/{id}", Name = ProjectPageRouteName)]
        [HttpGet]
        public async Task<IActionResult> Project(int id)
        {
            var project = await _projectsRepository.GetProjectAsync(id);

            if (project is null)
            {
                return NotFound();
            }

            var persons = await _personsRepository.GetEmployeesAsync();
            ViewBag.Employees = persons;

            LoadMessage();

            return View("Project", project);
        }

        /// <summary>
        /// Add employee with id <paramref name="memberId"/> to list of project <paramref name="projectId"/> members.
        /// </summary>
        /// <param name="projectId">Project identifier.</param>
        /// <param name="memberId">Employee identifier.</param>
        [Route("project/{projectId}/addMember/{memberId}", Name = AddEmployeeToProjectMembersRouteName)]
        [HttpGet]
        public async Task<IActionResult> AddMember(int projectId, int memberId)
        {
            var project = await _projectsRepository.GetProjectAsync(projectId);
            if (project is null)
            {
                return NotFound();
            }

            var employee = await _personsRepository.GetEmployeeAsync(memberId);
            if (employee is null)
            {
                return NotFound();
            }

            await _projectsRepository.AddProjectMemberAsync(project, employee);

            return RedirectToRoute(ProjectPageRouteName, new { id = project.Id });
        }

        /// <summary>
        /// Remove employee with id <paramref name="memberId"/> from list of project <paramref name="projectId"/> members.
        /// </summary>
        /// <param name="projectId">Project identifier.</param>
        /// <param name="memberId">Employee identifier.</param>
        [Route("project/{projectId}/removeMember/{memberId}", Name = RemoveEmployeeFromProjectMembersRouteName)]
        [HttpGet]
        public async Task<IActionResult> RemoveMember(int projectId, int memberId)
        {
            var project = await _projectsRepository.GetProjectAsync(projectId);
            if (project is null)
            {
                return NotFound();
            }

            var employee = await _personsRepository.GetEmployeeAsync(memberId);
            if (employee is null)
            {
                return NotFound();
            }

            if (project.LeaderId != employee.Id)
            {
                await _projectsRepository.RemoveProjectMemberAsync(project, employee);
            }
            else
            {
                SaveMessage("Leader of project can not be removed from project members list. Change project leader and try again.");
            }

            return RedirectToRoute(ProjectPageRouteName, new { id = project.Id });
        }

        /// <summary>
        /// Makes employee with id <paramref name="memberId"/> leader of project <paramref name="projectId"/>.
        /// </summary>
        /// <param name="projectId">Project identifier.</param>
        /// <param name="memberId">Employee identifier.</param>
        [Route("project/{projectId}/makeLeader/{memberId}", Name = SetLeaderOfProjectRouteName)]
        [HttpGet]
        public async Task<IActionResult> SetLeader(int projectId, int memberId)
        {
            var project = await _projectsRepository.GetProjectAsync(projectId);
            if (project is null)
            {
                return NotFound();
            }

            var employee = await _personsRepository.GetEmployeeAsync(memberId);
            if (employee is null)
            {
                return NotFound();
            }

            await _projectsRepository.SetLeader(project, employee);

            return RedirectToRoute(ProjectPageRouteName, new { id = project.Id });
        }

        #endregion Public Methods

        /// <summary>
        /// Loads message from session to ViewBag.
        /// </summary>
        public void LoadMessage()
        {
            var session = Request.HttpContext.Session;
            ViewBag.Message = session.GetString("message");
            session.Remove("message");
        }

        /// <summary>
        /// Store message in session storage.
        /// </summary>
        /// <param name="message">message.</param>
        private void SaveMessage(string message)
        {
            var session = Request.HttpContext.Session;
            session.SetString("message", message);
        }
    }
}
