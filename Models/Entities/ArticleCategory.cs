using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Narratify.Models.Entities;

public class ArticleCategory
{
    public int ArticleId { get; set; }
    public int CategoryId { get; set; }

    // Extra columns present in migration
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ArticleId))]
    public virtual Article Article { get; set; } = null!;

    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; } = null!;
}