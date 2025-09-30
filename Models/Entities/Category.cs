using System.ComponentModel.DataAnnotations;

namespace Narratify.Models.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [StringLength(100)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(7)]
    public string? Color { get; set; }

    public int ArticleCount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();
    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}