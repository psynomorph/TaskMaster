using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text.Encodings.Web;

namespace TaskMaster.TagHelpers
{
    /// <summary>
    /// Tag helper for pagination. Creates bootstrap 4 pagination layout.
    /// </summary>
    public class PaginationTagHelper : TagHelper
    {
        #region Public Properties

        /// <summary>
        /// Count of items.
        /// </summary>
        public int ItemsCount { get; set; } = 0;

        /// <summary>
        /// Count of items on page.
        /// </summary>
        public int ItemsOnPage { get; set; } = 20;

        /// <summary>
        /// Number if page.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Name of page parameter.
        /// </summary>
        public string PageParamName { get; set; }

        /// <summary>
        /// Uri.
        /// </summary>
        public string PageUri { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates bootstrap4 pagination layout.
        /// </summary>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            int pagesCount = ItemsCount / ItemsOnPage + (ItemsCount % ItemsOnPage > 0 ? 1 : 0);

            output.TagName = "ul";
            output.AddClass("pagination", HtmlEncoder.Default);

            TagBuilder prevBuilder = new TagBuilder("li");
            prevBuilder.AddCssClass("page-item");
            if (PageNumber == 1)
            {
                prevBuilder.AddCssClass("disabled");
            }
            var link = new TagBuilder("a");
            link.Attributes.Add("href", GetLink(1));
            link.AddCssClass("page-link");
            link.InnerHtml.Append("Prevoius");
            prevBuilder.InnerHtml.AppendHtml(link);

            output.Content.AppendHtml(prevBuilder);

            for(int pageNumber = 1; pageNumber <= pagesCount; ++pageNumber)
            {
                var pageItemBuilder = new TagBuilder("li");
                pageItemBuilder.AddCssClass("page-item");
                if (PageNumber == pageNumber)
                {
                    pageItemBuilder.AddCssClass("active");
                }
                link = new TagBuilder("a");
                link.AddCssClass("page-link");
                link.Attributes.Add("href", GetLink(pageNumber));
                link.InnerHtml.Append(pageNumber.ToString());

                pageItemBuilder.InnerHtml.AppendHtml(link);

                output.Content.AppendHtml(pageItemBuilder);
            }

            TagBuilder nextBuilder = new TagBuilder("li");
            nextBuilder.AddCssClass("page-item");
            if (PageNumber == pagesCount)
            {
                nextBuilder.AddCssClass("disabled");
            }
            link = new TagBuilder("a");
            link.AddCssClass("page-link");
            link.Attributes.Add("href", GetLink(pagesCount));
            link.InnerHtml.Append("Next");
            nextBuilder.InnerHtml.AppendHtml(link);

            output.Content.AppendHtml(nextBuilder);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Returns link with modified page parameter.
        /// </summary>
        /// <param name="pageNumber">Number of page.</param>
        /// <returns>Url of page.</returns>
        private string GetLink(int pageNumber)
        {
            var uri = new Uri(PageUri);
            var baseUri = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
            var query = QueryHelpers.ParseQuery(uri.Query);
            query["page"] = pageNumber.ToString();
            var qb = new QueryBuilder(query);
            return baseUri + qb.ToQueryString();
        }

        #endregion Private Methods
    }
}
