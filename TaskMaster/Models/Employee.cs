using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMaster.Models
{
    /// <summary>
    /// Represents o person.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// First name of person.
        /// </summary>
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of person.
        /// </summary>
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        /// <summary>
        /// Persons patronymic name. Can be <see langword="null"/> if person does not have it.
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Email address of person.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// List of projects in which the user participates.
        /// </summary>
        public ICollection<Project> Projects { get; set; }

        /// <summary>
        /// List of projects in which the user leads.
        /// </summary>
        public ICollection<Project> ProjectsWithLeadership { get; set; }

        /// <summary>
        /// Checks if this a new employee.
        /// </summary>
        /// <returns><see langword="true"/> if this employee is new. Otherwise <see langword="false"/>.</returns>
        public bool IsNew()
        {
            return Id == 0;
        }
    }
}
