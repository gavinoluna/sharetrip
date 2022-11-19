using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

using static SharedTrip.Infrastructure.Data.Constants.DataConstants.User;

namespace SharedTrip.Core.Models.User
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = null!;

        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLenght, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Please select profile image")]
        [Display(Name = "Profile picture")]
        public IFormFile ProfilePicture { get; set; } = null!;

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMaxLength, ErrorMessage = "The {0} must be exactly {1} characters.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;
    }
}