
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Identities
{
    public class PasswordChangeDto
    {
		public string UserId { get; set; }
		public string CurrentPassword { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(NewPassword), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
