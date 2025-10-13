using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Narratify.Models.Entities;

public class Comment
{
    [Key] public int Id { get; set; }

    // Content
    [Required]
    [StringLength(2000, ErrorMessage = "Comment cannot exceed 2000 characters")]
    [Display(Name = "Comment")]
    public string Content { get; set; } = string.Empty;

    /* <---- User Information ----> */
    [Display(Name = "User ID")] public string? UserId { get; set; }

    [ForeignKey("UserId")] public virtual User? User { get; set; }

    /* <--- Article Information ---> */
    [Required]
    [Display(Name = "Article ID")]
    public int ArticleId { get; set; }

    [ForeignKey("ArticleId")] public virtual Article Article { get; set; } = null!;

    /* <---- Timestamps ----> */
    [Display(Name = "Created At")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Updated At")] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Edited At")] public DateTime? EditedAt { get; set; }

    /* <---- Computed Properties ----> */
    [NotMapped]
    [Display(Name = "Author Name")]
    public string AuthorName => User?.DisplayName ?? "Anonymous";

    [NotMapped]
    [Display(Name = "Author Email")]
    public string AuthorEmail => User?.Email ?? string.Empty;

    [NotMapped]
    [Display(Name = "Time Ago")]
    public string TimeAgo
    {
        get
        {
            var timeSpan = DateTime.UtcNow - CreatedAt;
            return timeSpan.Days switch
            {
                > 365 => $"{timeSpan.Days / 365} year(s) ago",
                > 30 => $"{timeSpan.Days / 30} month(s) ago",
                > 7 => $"{timeSpan.Days / 7} week(s) ago",
                > 0 => $"{timeSpan.Days} day(s) ago",
                _ => timeSpan.Hours > 0 ? $"{timeSpan.Hours} hour(s) ago" :
                    timeSpan.Minutes > 0 ? $"{timeSpan.Minutes} minute(s) ago" : "Just now"
            };
        }
    }

    public void Edit(string newContent)
    {
        Content = newContent;
        EditedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool CanBeEditedBy(string userId)
    {
        return UserId == userId &&
               (DateTime.UtcNow - CreatedAt).TotalMinutes <= 15;
    }

    public bool CanBeDeletedBy(string userId)
    {
        return UserId == userId;
    }
}