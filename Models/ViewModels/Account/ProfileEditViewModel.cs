using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Narratify.Models.ViewModels.Account
{
    public class ProfileEditViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        [Display(Name = "Bio")]
        public string? Bio { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }

        public string? CurrentProfilePictureUrl { get; set; }

        // Computed properties
        public string DisplayName => !string.IsNullOrEmpty(FirstName) ? $"{FirstName} {LastName}".Trim() : "Anonymous";
        
        public string Initials => $"{FirstName.FirstOrDefault()}{LastName.FirstOrDefault()}".ToUpper();

        // Method to get profile picture with fallback
        public string GetProfilePictureOrDefault()
        {
            if (!string.IsNullOrEmpty(CurrentProfilePictureUrl))
            {
                return CurrentProfilePictureUrl;
            }
            
            var initials = System.Uri.EscapeDataString(Initials);
            return $"https://ui-avatars.com/api/?name={initials}&background=667eea&color=fff&size=150";
        }
    }
}
