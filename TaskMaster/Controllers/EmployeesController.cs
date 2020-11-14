using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskMaster.Models;
using TaskMaster.Services;

using static TaskMaster.Helpers.RouteNames;

namespace TaskMaster.Controllers
{
    /// <summary>
    /// Employees controller
    /// </summary>
    public class EmployeesController : Controller
    {
        #region Private Fields

        /// <summary>
        /// Employees repository.
        /// </summary>
        private readonly IEmployeeRepository _employeeRepository;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates employees controller.
        /// </summary>
        /// <param name="employeeRepository">Employees repository.</param>
        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Returns view for employee adding page.
        /// </summary>
        [Route("employee/add", Name = AddEmployeePageRouteName)]
        [HttpGet]
        public IActionResult Add()
        {
            var employee = new Employee();
            return View("Edit", employee);
        }

        /// <summary>
        /// Adds employee model to repository and redirects to employee page.
        /// if model is invalid returns view for continue editing.
        /// </summary>
        /// <param name="employee">Employee model.</param>
        [Route("employee/add", Name = AddEmployeeEndpointRouteName)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", employee);
            }

            await _employeeRepository.SaveEmployeeAsync(employee);
            return RedirectToAction(nameof(Employee), new { id = employee.Id });
        }

        /// <summary>
        /// Deletes employee with <paramref name="id"/> identifier and redirects to employees list page.
        /// </summary>
        /// <param name="id">Id of employee to remove.</param>
        [Route("employee/{id}/delete", Name = DeleteEmployeeRouteName)]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);
            if (employee is null)
            {
                return NotFound();
            }
            await _employeeRepository.DeleteEmployeeAsync(employee);
            return RedirectToRoute("EmployeesList");
        }

        /// <summary>
        /// Returns editing page for employee with <paramref name="id"/> identifier.
        /// </summary>
        /// <param name="id">Employees identifier.</param>
        [Route("employee/{id}/edit", Name = EditEmployeePageRouteName)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);
            if (employee is null)
            {
                return NotFound();
            }

            return View(employee);
        }

        /// <summary>
        /// Update employee model in repository and redirects to his page.
        /// Of model is invalid returns view to continue editing.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="employee">Received employee model.</param>
        [Route("employee/{id}/edit", Name = UpdateEmployeeEndpointRouteName)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            await _employeeRepository.SaveEmployeeAsync(employee);
            return RedirectToAction(nameof(Employee), new { id });
        }

        /// <summary>
        /// Returns page with employee data.
        /// </summary>
        /// <param name="id">Id of employee.</param>
        [Route("employee/{id}", Name = EmployeePageRouteName)]
        [HttpGet]
        public async Task<IActionResult> Employee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);

            if (employee is null)
            {
                return NotFound();
            }

            return View("employee", employee);
        }

        /// <summary>
        /// Returns list of employees.
        /// </summary>
        [Route("employees", Name = EmployeeListRouteName)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            return View(employees);
        }

        #endregion Public Methods
    }
}
