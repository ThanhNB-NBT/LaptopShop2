using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Html;
using X.PagedList;
using System.Text;

namespace LaptopShop2.Functions
{
    public static class PaginationHelper
    {
        public static IHtmlContent CustomPagedListPager<T>(this IHtmlHelper html, IPagedList<T> list, Func<int, string> generatePageUrl)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var result = new StringBuilder();

            // Previous page
            if (list.HasPreviousPage)
            {
                result.AppendFormat("<a href='{0}'><i class='fa fa-long-arrow-left'></i></a>", generatePageUrl(list.PageNumber - 1));
            }

            // Page numbers
            for (int i = 1; i <= list.PageCount; i++)
            {
                if (i == list.PageNumber)
                {
                    result.AppendFormat("<a href='{0}' >{1}</a>", generatePageUrl(i), i);
                }
                else
                {
                    result.AppendFormat("<a href='{0}' >{1}</a>", generatePageUrl(i), i);
                }
            }

            // Next page
            if (list.HasNextPage)
            {
                result.AppendFormat("<a href='{0}'><i class='fa fa-long-arrow-right'></i></a>", generatePageUrl(list.PageNumber + 1));
            }

            return new HtmlString(result.ToString());
        }
    }

}
