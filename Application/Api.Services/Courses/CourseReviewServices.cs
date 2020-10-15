using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using MediatR;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Application.Common;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Api.Services.Courses
{
	public class CourseReviewServices: BaseService, ICourseReviewServices
    {
		private readonly ICourseRepository _courseRepository;
		private readonly ICourseReviewRepository _courseReviewRepository;

		public CourseReviewServices(
			ICourseRepository courseRepository,
			ICourseReviewRepository courseReviewRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_courseRepository = courseRepository;
			_courseReviewRepository = courseReviewRepository;
        }
        
		public async Task<PaginationDto<CourseReviewDto>> GetCourseReviewsAsync(int courseId, int pageNumber, int pageSize)
		{
			var courseReviews = await _courseReviewRepository.GetCourseReviewsAsync(courseId, pageNumber, pageSize);
            var paginationCourseReviews = Mapper.Map<PaginationDto<CourseReviewDto>>(courseReviews);
			return paginationCourseReviews;
		}

		public async Task<CourseReviewDto> GetCourseReviewForCurrentUserAsync(int courseId)
        {
			var user = await GetCurrentUser();
            var courseReview = await _courseReviewRepository.GetCourseReviewByCourseIdAndUserIdAsync(user.Id, courseId);
			return Mapper.Map<CourseReviewDto>(courseReview);
        }

		public async Task<CourseReviewDto> CreateCourseReviewAsync(int courseId, CourseReviewCreateRequestDto request)
        {
			// 1. check user and course
			var user = await GetCurrentUser();
			var course = await _courseRepository.GetCourseAsync(courseId);
            if(course == null)
            {
              throw new NotFoundException("Course not found");
            }
            // 2. check user's exsisting review
			var review = await _courseReviewRepository.GetCourseReviewByCourseIdAndUserIdAsync(user.Id, courseId);
			if (review != null)
            {
				throw new BadRequestException("You have reviewed this course already.");
            }
			// 3. create review and update course
			var newReview = Review.Create(course, user, request.Comment, request.Score);
			await _courseReviewRepository.CreateAsync(newReview);
			await _courseReviewRepository.SaveAsync();
			return Mapper.Map<CourseReviewDto>(newReview);
        }

		public async Task<CourseReviewDto> UpdateCourseReviewForCurrentUserAsync(int courseId, CourseReviewUpdateRequestDto request)
		{
			// 1. check user and course
			var user = await GetCurrentUser();
			var course = await _courseRepository.GetCourseAsync(courseId);
            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }
			//2. update course rating
			var review = await _courseReviewRepository.GetCourseReviewByCourseIdAndUserIdAsync(user.Id, courseId);
			review.Update(user, request.Comment, request.Score);
			await _courseReviewRepository.SaveAsync();
			return Mapper.Map<CourseReviewDto>(review);
        }

		public async Task DeleteCourseReviewForCurrentUserAsync(int courseId)
		{
			// 1. check user and course
            var user = await GetCurrentUser();
            var course = await _courseRepository.GetCourseAsync(courseId);
            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }
			// 2. remove review
			var review = await _courseReviewRepository.GetCourseReviewByCourseIdAndUserIdAsync(user.Id, courseId);
			_courseReviewRepository.Remove(review);
			await _courseReviewRepository.SaveAsync();
        }
  
		public async Task AddReviewLikeAsync(int courseId, int reviewId) 
		{
			var user = await GetCurrentUser();
			var course = await _courseRepository.GetCourseAsync(courseId);
            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }
			var review = await _courseReviewRepository.GetCourseReviewByIdAsync(reviewId);
			review.AddLike(user);
			await _courseReviewRepository.SaveAsync();
		}

		public async Task RemoveReviewLikeAsync(int courseId, int reviewId)
        {
            var user = await GetCurrentUser();
            var course = await _courseRepository.GetCourseAsync(courseId);
            if (course == null)
            {
                throw new NotFoundException("Course not found");
            }
            var review = await _courseReviewRepository.GetCourseReviewByIdAsync(reviewId);
            review.RemoveLike(user);
            await _courseReviewRepository.SaveAsync();
        }
    }
}
