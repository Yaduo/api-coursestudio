using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CourseStudio.Application.Dtos.Users
{
	public class UserUpdateFormRequestDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
    }
}
