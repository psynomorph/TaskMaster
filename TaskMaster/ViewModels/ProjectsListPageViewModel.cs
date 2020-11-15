using System.Collections.Generic;
using TaskMaster.Models;

namespace TaskMaster.ViewModels
{
    /// <summary>
    /// View model for projects list page.
    /// </summary>
    public class ProjectsListPageViewModel
    {
        /// <summary>
        /// List of projects.
        /// </summary>
        public IEnumerable<Project> Projects { get; set; }

        /// <summary>
        /// Sorting state.
        /// </summary>
        public SortState Sort { get; set; }

        /// <summary>
        /// Info about pagination.
        /// </summary>
        public PageInfo PageInfo { get; set; }

        /// <summary>
        /// Summary count of projects.
        /// </summary>
        public int ProjectsCount { get; set; }

        /// <summary>
        /// Information about filtering.
        /// </summary>
        public FilteringModel Filtering { get; set; }
    }
}
