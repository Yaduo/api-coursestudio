using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Events.Trades;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Trades;

namespace CourseStudio.Domain.Services.Users
{
	public class UserCourseEnrollWhenOrderCompleteEventHandler : IAsyncNotificationHandler<OrderCompleteDomainEvent>
	{
		private readonly ICourseRepository _courseRepository;

		public UserCourseEnrollWhenOrderCompleteEventHandler(
			ICourseRepository courseRepository
		)
		{
			_courseRepository = courseRepository;
		}

		public async Task Handle(OrderCompleteDomainEvent @event)
		{
			// 1. get order
			Order order = (Order)@event.Order;

			// 2. get user from order
			ApplicationUser user = order.User;

			// 3. enroll courses
			var courseIds = order.OrderItems.Select(i => i.CourseId).ToList();
			var courses = await _courseRepository.GetCoursesByIdsAsync(courseIds);
            foreach (var course in courses)
            {
                course.Enroll(user, order);
            }
		}
	}
}
