using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApplication12.TagHelpers
{
    [HtmlTargetElement("blog-preview")]
    public class BlogPostPreviewTagHelper : TagHelper
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int MaxLength { get; set; } = 150;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "card mb-3 p-3");

            string truncated = Content.Length > MaxLength 
                ? Content.Substring(0, MaxLength) + "..." 
                : Content;

            output.Content.SetHtmlContent($"<h3>{Title}</h3><p>{truncated}</p>");
        }
    }
}