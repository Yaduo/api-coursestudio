
using System;
using System.Collections.Generic;
using CourseStudio.Application.Dtos.Courses;

namespace CourseStudio.Application.Dtos.Users
{
    public class AdminDto
    {
		public int? Id { get; set; }
		public string Email { get; set; }
		public IList<string> Roles { get; set; }
		public DateTime CreateDateUTC { get; set; }
        public bool IsActivated { get; set; }
    }
}
