using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskMaster.Models;

namespace TaskMaster.Services
{
    /// <summary>
    /// Employee repository uses EF Core Database context to store employee data.
    /// </summary>
    public class EmployeeEfRepository : IEmployeeRepository
    {
        #region Private Fields

        /// <summary>
        /// Database context.
        /// </summary>
        private readonly DatabaseContext _db;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="db">Database context.</param>
        public EmployeeEfRepository(DatabaseContext db)
        {
            _db = db;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <inheritdoc/>
        public async Task DeleteEmployeeAsync(Employee employee)
        {
            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Employee> GetEmployeeAsync(int id)
        {
            return await _db.Employees
                .Where(person => person.Id == id)
                .Include(person => person.Projects)
                .Include(person => person.ProjectsWithLeadership)
                .SingleOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _db.Employees
                .Include(person => person.Projects)
                .Include(person => person.ProjectsWithLeadership)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task SaveEmployeeAsync(Employee employee)
        {
            if (employee.IsNew())
            {
                await AddEmployeeAsync(employee);
            }
            else
            {
                await UpdateEmployeeAsync(employee);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Add employee to database.
        /// </summary>
        /// <param name="employee">Employee to add.</param>
        private async ValueTask AddEmployeeAsync(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Update employee in database.
        /// </summary>
        /// <param name="employee">Employee model with new data.</param>
        private async ValueTask UpdateEmployeeAsync(Employee employee)
        {
            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();
        }

        #endregion Private Methods
    }
}
