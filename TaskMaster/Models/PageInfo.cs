namespace TaskMaster.Models
{
    /// <summary>
    /// Information about page.
    /// </summary>
    public class PageInfo
    {
        #region Public Constructors

        /// <summary>
        /// Creates page info object.
        /// </summary>
        public PageInfo() { }

        /// <summary>
        /// Creates page info object.
        /// </summary>
        /// <param name="pageNumber">Number of page.</param>
        /// <param name="elementsOnPage">Count of elements on page.</param>
        public PageInfo(int pageNumber, int elementsOnPage)
        {
            PageNumber = pageNumber;
            ElementsOnPage = elementsOnPage;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Count of elements of page.
        /// </summary>
        public int ElementsOnPage { get; set; } = 20;

        /// <summary>
        /// Page number.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns count of pages.
        /// </summary>
        /// <param name="elementsCount">Count of elements.</param>
        /// <returns>Count of pages.</returns>
        public int GetPagesCount(int elementsCount)
        {
            return elementsCount / ElementsOnPage + (elementsCount % ElementsOnPage > 0 ? 1 : 0);
        }

        #endregion Public Methods
    }
}
