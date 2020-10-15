using System;
using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Courses;

namespace CourseStudio.Domain.Repositories.Courses
{
	public interface ILectureRepository : IRepository<Lecture>
	{
		Task<Lecture> GetLectureAsync(int lectureId);
	}
}
