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

    [Display(Name = "HTML Content")] public string? HtmlContent { get; set; }

    /* <---- User Information ----> */
    [Display(Name = "User ID")] public string? UserId { get; set; }

    [ForeignKey("UserId")] public virtual User? User { get; set; }

    /* <--- Article Information ---> */
    [Required]
    [Display(Name = "Article ID")]
    public int ArticleId { get; set; }

    [ForeignKey("ArticleId")] public virtual Article Article { get; set; } = null!;

    /* <---- Threading Support ----> */
    [Display(Name = "Parent Comment ID")] public int? ParentCommentId { get; set; }

    [ForeignKey("ParentCommentId")] public virtual Comment? ParentComment { get; set; }

    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();

    /* <---- Timestamps ----> */
    [Display(Name = "Created At")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Updated At")] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Edited At")] public DateTime? EditedAt { get; set; }

    /* <--- Engagement Metrics ----> */
    [Display(Name = "Like Count")] public int LikeCount { get; set; }

    [Display(Name = "Dislike Count")] public int DislikeCount { get; set; }

    [Display(Name = "Reply Count")] public int ReplyCount { get; set; }

    /* <---- Computed Properties ----> */
    [NotMapped]
    [Display(Name = "Author Name")]
    public string AuthorName => User?.DisplayName ?? "Anonymous";

    [NotMapped]
    [Display(Name = "Author Email")]
    public string AuthorEmail => User?.Email ?? string.Empty;

    [NotMapped]
    [Display(Name = "Thread Level")]
    public int ThreadLevel
    {
        get
        {
            var level = 0;
            var current = ParentComment;
            while (current != null)
            {
                level++;
                current = current.ParentComment;
            }

            return level;
        }
    }

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

    public void IncrementLikes()
    {
        LikeCount++;
    }

    public void IncrementDislikes()
    {
        DislikeCount++;
    }

    public void IncrementReplyCount()
    {
        ReplyCount++;
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

    public List<Comment> GetAllDescendants()
    {
        var descendants = new List<Comment>();
        foreach (var reply in Replies)
        {
            descendants.Add(reply);
            descendants.AddRange(reply.GetAllDescendants());
        }

        return descendants;
    }
}