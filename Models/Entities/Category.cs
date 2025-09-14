using System.ComponentModel.DataAnnotations;

namespace Narratify.Models.Entities;

public class Category
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [StringLength(100)]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = string.Empty;

    [StringLength(7)]
    [Display(Name = "Color")]
    public string? Color { get; set; } = "#007bff";

    [Display(Name = "Article Count")] public int ArticleCount { get; set; } = 0;

    [Display(Name = "Created At")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
    public virtual ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();
}