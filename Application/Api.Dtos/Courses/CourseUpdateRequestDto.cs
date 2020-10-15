using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseStudio.Application.Dtos.Courses
{
	public class  CourseUpdateRequestDto 
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
		public int? LanguageTypeId { get; set; }
		public string CoverPageImage { get; set; }
        public string Prerequisites { get; set; }
        public string Refferences { get; set; }
        public string TargetStudents { get; set; }
        [Range(0.0, Double.MaxValue)]
		public decimal? UnitPrice { get; set; }
		public decimal? DiscountAmount { get; set; }
		public double? DiscountPercent { get; set; }
        public IList<int> CourseAttributeIds { get; set; }
    }
}
