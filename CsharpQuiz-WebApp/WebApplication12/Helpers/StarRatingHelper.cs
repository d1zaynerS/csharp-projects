using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication12.Helpers
{
    public static class StarRatingHelper
    {
        public static HtmlString StarRating(this IHtmlHelper html, int rating)
        {
            if (rating < 0) rating = 0;
            if (rating > 5) rating = 5;

            string stars = "";
            for (int i = 1; i <= 5; i++)
            {
                stars += (i <= rating) ? "★" : "☆";
            }

            return new HtmlString($"<span style=\"color: gold; font-size: 22px;\">{stars}</span>");
        }
    }
}