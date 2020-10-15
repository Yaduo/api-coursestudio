using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CourseStudio.Application.Dtos.Courses
{
    public class ImageCreateFormRequestDto
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}
