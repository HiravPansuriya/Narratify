using System.ComponentModel.DataAnnotations;

namespace Narratify.Models.ViewModels.Articles
{
    public class PostCommentViewModel
    {
        [Required]
        public int ArticleId { get; set; }
        
        [Required]
        public string ArticleSlug { get; set; } = string.Empty;
        
        [Required]
        [StringLength(2000, ErrorMessage = "Comment cannot exceed 2000 characters")]
        [Display(Name = "Comment")]
        public string NewCommentContent { get; set; } = string.Empty;
    }
}
