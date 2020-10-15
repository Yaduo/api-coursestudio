using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Courses;

namespace CourseStudio.Api.Services.Courses
{
    public delegate VideoDto VideoPatchApplyDelegate(VideoDto dto);

    public interface IVideoServices
    {
		Task<VideoDto> GetVideByLectureAsync(int lectureId);
		Task<VimeoVidoeResponseDto> CreateVideoUploadTicketAsync(int lectureId, VidoeUploadTicketRequestDto request);
		Task<VimeoVidoeResponseDto> GetVimeoVideoStutasByIdAsync(int videoId);
		Task SynchronizeVideoAsync(int videoId);
		Task DeleteVideoAsync(int videoId); 
		Task<VidoeTextTracksUploadUploadTicketResponseDto> CreateTextTracksUploadTicketAsync(int videoId, VidoeTextTracksUploadTicketRequestDto request);
		Task<VidoeTextTracksResponseDto> GetAllTextTracks(int videoId);
		Task DeleteTextTrackAsync(int videoId, int textTrackId);
    }
}
