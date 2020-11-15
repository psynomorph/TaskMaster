using System.ComponentModel.DataAnnotations;
using TaskMaster.Models;

namespace TaskMaster.Validation
{
    /// <summary>
    /// Validation attribute. Checks project completion date not early then beginning date,
    /// </summary>
    public class CompletionDateNotEarlyThenBeggingDateAttribute : ValidationAttribute
    {
        #region Public Constructors

        /// <summary>
        /// Creates attribute.
        /// </summary>
        public CompletionDateNotEarlyThenBeggingDateAttribute()
        {
            ErrorMessage = "Project completion date cannot be earlier than beginning date.";
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Checks project is valid.
        /// </summary>
        /// <param name="value">Project.</param>
        /// <returns>Result of check.</returns>
        public override bool IsValid(object value)
        {
            var project = value as Project;

            if (project.CompletionDate < project.BegginingDate)
            {
                return false;
            }

            return true;
        }

        #endregion Public Methods
    }
}
