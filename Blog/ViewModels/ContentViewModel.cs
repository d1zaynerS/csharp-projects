using System.Collections.Generic;
using Blog.Models;

namespace Blog.ViewModels
{
    public class ContentViewModel
    {
        public IEnumerable<Publication> Publications { get; set; } = new List<Publication>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}