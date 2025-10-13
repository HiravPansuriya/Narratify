using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Narratify.Models.Entities;

public class User : IdentityUser
{
    /* <---- Personal Information ----> */
    [Required]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
    [Display(Name = "Bio")]
    public string? Bio { get; set; }

    [StringLength(255)]
    [Display(Name = "Profile Picture")]
    public string? ProfilePictureUrl { get; set; }


    /* <---- Account Metadata ----> */
    [Display(Name = "Date Joined")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ArticleCount { get; set; } = 0;

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    [Display(Name = "Full Name")] public string FullName => $"{FirstName} {LastName}".Trim();

    [Display(Name = "Display Name")]
    public string DisplayName => !string.IsNullOrEmpty(FirstName) ? FullName : UserName ?? Email ?? "Anonymous";

    [Display(Name = "Initials")]
    public string Initials => $"{FirstName.FirstOrDefault()}{LastName.FirstOrDefault()}".ToUpper();
    
    public DateTime? LastLoginAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public string GetProfilePictureOrDefault()
    {
        if (!string.IsNullOrEmpty(ProfilePictureUrl))
        {
            return ProfilePictureUrl;
        }
        
        // Generate initials-based avatar using UI Avatars service
        var initials = System.Uri.EscapeDataString(Initials);
        return $"https://ui-avatars.com/api/?name={initials}&background=667eea&color=fff&size=150";
    }
}