using System;
using System.Collections.Generic;

namespace Blog.Models
{
    public class Publication
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? SeoDescription { get; set; }
        public string? Keywords { get; set; }
        public string Content { get; set; } = null!;
        public int TotalViews { get; set; }
        public string? ImagePath { get; set; }
        public string? Image { get; set; }
        public string? FullImageName { get; set; }

        public List<Category> Categories { get; set; } = new();
    }
}