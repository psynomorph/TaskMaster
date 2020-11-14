using System.ComponentModel.DataAnnotations;
using TaskMaster.Models;

namespace TaskMaster.Validation
{
    public class CompletionDateNotEarlyThenBeggingDateAttribute : ValidationAttribute
    {
        public CompletionDateNotEarlyThenBeggingDateAttribute()
        {
            ErrorMessage = "Project completion date cannot be earlier than beggining date.";
        }

        public override bool IsValid(object value)
        {
            var project = value as Project;

            if (project.CompletionDate < project.BegginingDate)
            {
                return false;
            }

            return true;
        }
    }
}
