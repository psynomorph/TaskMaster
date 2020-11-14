using System.Collections.Generic;
using System.Threading.Tasks;
using TaskMaster.Models;

namespace TaskMaster.Services
{
    /// <summary>
    /// Employees repository.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Deletes employee.
        /// </summary>
        /// <param name="employee">Employee to delete.</param>
        Task DeleteEmployeeAsync(Employee employee);

        /// <summary>
        /// Return employee by his id.
        /// </summary>
        /// <param name="id">Id of employee.</param>
        /// <returns>Employee model.</returns>
        Task<Employee> GetEmployeeAsync(int id);

        /// <summary>
        /// Returns collection of all employees.
        /// </summary>
        /// <returns>Collection of employees.</returns>
        Task<IEnumerable<Employee>> GetEmployeesAsync();

        /// <summary>
        /// Saves employee.
        /// </summary>
        /// <param name="employee">Employee to save.</param>
        Task SaveEmployeeAsync(Employee employee);
    }
}