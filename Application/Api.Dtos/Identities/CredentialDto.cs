using System.ComponentModel.DataAnnotations;
using CourseStudio.Lib.Utilities.Identity;

namespace CourseStudio.Application.Dtos.Identities
{
    public class CredentialDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
		public string UserName { get { return UserNameHelper.GenerateUserNameFromEmail(Email); }}
        
    }
}

