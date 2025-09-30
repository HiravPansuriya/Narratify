using System.ComponentModel.DataAnnotations;

namespace Narratify.Models.ViewModels.Articles
{
    public class EditArticleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        [Display(Name = "Content")]
        public string Content { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Summary cannot exceed 500 characters")]
        [Display(Name = "Summary")]
        public string? Summary { get; set; }

        [StringLength(500)]
        [Display(Name = "Featured Image URL")]
        public string? FeaturedImageUrl { get; set; }

        [Display(Name = "Publish Article")]
        public bool IsPublished { get; set; }

        // Read-only properties for display
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}
