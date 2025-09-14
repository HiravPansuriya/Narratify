using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Narratify.Models.Entities;

public class Article
{
    [Key] public int Id { get; set; }

    /* <---- Article Information ----> */
    [Required]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Required] [Display(Name = "Content")] public string Content { get; set; } = string.Empty;

    [Display(Name = "HTML Content")] public string HtmlContent { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Summary cannot exceed 500 characters")]
    [Display(Name = "Summary")]
    public string? Summary { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = string.Empty;

    /* <---- Featured Image ----> */
    [StringLength(500)]
    [Display(Name = "Featured Image URL")]
    public string? FeaturedImageUrl { get; set; }

    [StringLength(200)]
    [Display(Name = "Featured Image Alt Text")]
    public string? FeaturedImageAlt { get; set; }

    /* <---- Publishing Status ----> */
    [Display(Name = "Status")] public ArticleStatus Status { get; set; } = ArticleStatus.Draft;

    [Display(Name = "Is Published")] public bool IsPublished { get; set; }

    /* <---- Timestamps ----> */
    [Display(Name = "Created At")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Updated At")] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "Published At")] public DateTime? PublishedAt { get; set; }

    /* <---- Engagement Metrics ----> */
    [Display(Name = "View Count")] public int ViewCount { get; set; }

    [Display(Name = "Like Count")] public int LikeCount { get; set; } = 0;

    [Display(Name = "Comment Count")] public int CommentCount { get; set; } = 0;

    /* <---- Reading Time ----> */
    [Display(Name = "Estimated Reading Time (minutes)")]
    public int ReadingTimeMinutes { get; set; }

    /* <---- Author Information ----> */
    [Required] [Display(Name = "Author")] public string AuthorId { get; set; } = string.Empty;

    [ForeignKey("AuthorId")] public virtual User Author { get; set; } = null!;

    // Navigation Properties
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();

    /* <---- Computed Properties ----> */
    [NotMapped]
    [Display(Name = "Author Name")]
    public string AuthorName => Author?.DisplayName ?? "Unknown Author";

    [NotMapped]
    [Display(Name = "Published Status")]
    public string PublishedStatus => IsPublished ? "Published" : Status.ToString();

    [NotMapped]
    [Display(Name = "Time Ago")]
    public string TimeAgo
    {
        get
        {
            var timeSpan = DateTime.UtcNow - (PublishedAt ?? CreatedAt);
            return timeSpan.Days switch
            {
                > 365 => $"{timeSpan.Days / 365} year(s) ago",
                > 30 => $"{timeSpan.Days / 30} month(s) ago",
                > 7 => $"{timeSpan.Days / 7} week(s) ago",
                > 0 => $"{timeSpan.Days} day(s) ago",
                _ => timeSpan.Hours > 0 ? $"{timeSpan.Hours} hour(s) ago" : "Just now"
            };
        }
    }

    [NotMapped]
    [Display(Name = "Preview Text")]
    public string PreviewText
    {
        get
        {
            if (!string.IsNullOrEmpty(Summary))
                return Summary;

            var plainText = Regex.Replace(Content, @"<[^>]+>|[#*`]", "");
            return plainText.Length > 150 ? plainText.Substring(0, 150) + "..." : plainText;
        }
    }

    // Methods
    public void UpdateReadingTime()
    {
        if (!string.IsNullOrEmpty(Content))
        {
            var wordCount = Content.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Length;
            ReadingTimeMinutes =
                Math.Max(1, (int)Math.Ceiling(wordCount / 200.0)); // Average reading speed: 200 words/min
        }
    }

    public void GenerateSlug()
    {
        if (!string.IsNullOrEmpty(Title))
            Slug = Title.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace("\"", "")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("!", "")
                .Replace("?", "")
                .Replace(":", "")
                .Replace(";", "")
                .Trim('-');
    }

    public void Publish()
    {
        IsPublished = true;
        Status = ArticleStatus.Published;
        PublishedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unpublish()
    {
        IsPublished = false;
        Status = ArticleStatus.Draft;
        PublishedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementViewCount()
    {
        ViewCount++;
    }
}

public enum ArticleStatus
{
    [Display(Name = "Draft")] Draft = 0,

    [Display(Name = "Published")] Published = 1,

    [Display(Name = "Archived")] Archived = 2
}