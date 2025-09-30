using Narratify.Models.Entities;
using System.Collections.Generic;

namespace Narratify.Models.ViewModels.Articles
{
    public class BlogViewModel
    {
        public required Article Article { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public string NewCommentContent { get; set; } = string.Empty;
    }
}