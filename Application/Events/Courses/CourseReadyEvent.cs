using MediatR;

namespace CourseStudio.Application.Events.Courses
{
    public class CourseReadyEvent : IAsyncNotification
    {
        public int CourseId { get; private set; }

        public CourseReadyEvent(int courseId)
        {
            CourseId = courseId;
        }
    }
}