

using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Users
{
    public class ApplyForTutorDto
    {
        [Required]
        public string Resume { get; set; }
    }
}
