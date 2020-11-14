using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskMaster.Validation;

namespace TaskMaster.Models
{
    [CompletionDateNotEarlyThenBeggingDate]
    public class Project
    {
        /// <summary>
        /// Identifier of project.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Project name.
        /// </summary>
        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        /// <summary>
        /// Name of customer company.
        /// </summary>
        [Required]
        [MinLength(5)]
        public string CustomerCompanyName { get; set; }

        /// <summary>
        /// Name of executor company.
        /// </summary>
        [Required]
        [MinLength(5)]
        public string ExecutorCompanyName { get; set; }

        /// <summary>
        /// Date of project beginning.
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime BegginingDate { get; set; }

        /// <summary>
        /// Date of project completion.
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime CompletionDate { get; set; }

        /// <summary>
        /// Identifier of project leader.
        /// </summary>
        public int LeaderId { get; set; }

        /// <summary>
        /// Leader of project.
        /// </summary>
        [ForeignKey(nameof(LeaderId))]
        public Employee Leader { get; set; }

        /// <summary>
        /// Project members.
        /// </summary>
        public ICollection<Employee> Members { get; set; }

        /// <summary>
        /// Checks if this a new project.
        /// </summary>
        /// <returns><see langword="true"/> if this project is new. Otherwise <see langword="false"/>.</returns>
        public bool IsNew()
        {
            return Id == 0;
        }
    }
}
