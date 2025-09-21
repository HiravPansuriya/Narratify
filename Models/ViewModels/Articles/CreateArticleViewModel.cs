using System.ComponentModel.DataAnnotations;

namespace Narratify.Models.ViewModels
{
    public class CreateArticleViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
