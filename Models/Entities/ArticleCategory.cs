using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Narratify.Models.Entities;

public class ArticleCategory
{
    [Key] public int Id { get; set; }

    [Required] public int ArticleId { get; set; }

    [ForeignKey("ArticleId")] public virtual Article Article { get; set; } = null!;

    [Required] public int CategoryId { get; set; }

    [ForeignKey("CategoryId")] public virtual Category Category { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}