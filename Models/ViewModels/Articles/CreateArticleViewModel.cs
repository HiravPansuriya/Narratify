using System.ComponentModel.DataAnnotations;

namespace Narratify.Models.ViewModels.Articles
{
    public class CreateArticleViewModel
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Content { get; set; }

        public bool IsPublished { get; set; } = false;
    }
}
