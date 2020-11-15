using System;
using System.ComponentModel.DataAnnotations;

namespace TaskMaster.Models
{
    /// <summary>
    /// Model with filtering params.
    /// </summary>
    public class FilteringModel
    {
        /// <summary>
        /// Minimum value of completion date.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? MinCompletionDate { get; set; }

        /// <summary>
        /// Maximum value of completion date.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? MaxCompletionDate { get; set; }

        /// <summary>
        /// Minimum value of beginning date.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? MinBeginningDate { get; set; }

        /// <summary>
        /// Maximum value of beginning date.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? MaxBeginningDate { get; set; }

        /// <summary>
        /// Minimum value of priority.
        /// </summary>
        public int MinPriority { get; set; } = 1;

        /// <summary>
        /// Maximum value of priority.
        /// </summary>
        public int MaxPriority { get; set; } = 10;
    }
}
