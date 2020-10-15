using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Application.Dtos.CourseAttributes;

namespace CourseStudio.Application.Dtos.Courses
{
    public class CourseDto
    {
        public int Id { get; set; }
        [Required]
        public TutorDto Tutor { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public string Subtitle { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        // TODO: enum type check annotation
        public string LanguageType { get; set; }
		public DateTime CreateDateUTC { get; set; }
		public DateTime LastUpdateDateUTC { get; set; }
        public int TotalDurationInSeconds { get; set; }
        public int LecturesCount { get; set; }
        public double? Rating { get; set; }
        public int RatesCount { get; set; }
        public int EnrollmentCount { get; set; }
        public bool IsReady { get; set; }
        public bool IsActive { get; set; }
		public string State { get; set; }
        [Required]
        public string Prerequisites { get; set; }
        public string Refferences { get; set; }
        public string TargetStudents { get; set; }
		public string CoverPageImage { get; set; }
		[Range(0.0, 100.0)]
        public double DiscountPercent { get; set; }
        [Range(0.0, Double.MaxValue)]
        public decimal DiscountAmount { get; set; }
        public decimal UnitPrice { get; set; }
		public decimal Price { get; set; }
        public IList<SectionDto> Sections { get; set; }
        public IList<CourseAttributeDto> Attributes { get; set; }
		public string VimeoAlbumId { get; set; }
        public VideoDto PreviewVideo { get; set; }
    }
}
