using System.ComponentModel.DataAnnotations;

namespace TaskMaster.Models
{
    public enum SortState
    {
        [Display(Name = "Name ascending")]
        NameAsc,

        [Display(Name = "Name descending")]
        NameDesc,

        [Display(Name = "Priority ascending")]
        PriorityAsc,

        [Display(Name = "Priority descending")]
        PriorityDesc,

        [Display(Name = "Date of project beginning ascending")]
        BeginningDateAsc,

        [Display(Name = "Date of project beginning descending")]
        BeginningDateDesc,

        [Display(Name = "Date of project completion ascending")]
        CompletionDateAsc,

        [Display(Name = "Date of project completion descending")]
        CompletionDateDesc
    }
}
