using System.Threading.Tasks;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Domain.Repositories.Courses
{
	public interface IVideoRepository: IRepository<Video> 
    {
		Task<Video> GetVideoByIdAsync(int id);
        Task<Video> GetVideoByLectureAsync(int lectureId); 
    }
}
