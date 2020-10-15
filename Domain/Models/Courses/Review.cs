using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Exceptions.Courses;

namespace CourseStudio.Doamin.Models.Courses
{
    public class Review : Entity, IAggregateRoot
    {
		public Review()
        {
			this.Likes = new List<Like>();
        }

        /// <summary>
        /// entity properties
        /// </summary>
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string ReviewerId { get; set; }
		[MaxLength]
		public string Comment { get; set; }
		private double _score;
		public double Score 
		{ 
			get
			{
				return _score;
            }
			set
            {
				if (value > 5 || value < 0)
                {
                    throw new CourseReviewException("The rating score must be in 0 to 5");
                }
				_score = value;
            } 
		}
		public IList<Like> Likes { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime LastUpdateDateUTC { get; set; }

        /// Navigation properties
        public Course Course { get; set; }
		public ApplicationUser Reviewer { get; set; }

		public static Review Create(
            Course course,
            ApplicationUser reviewer,
			string comment,
			double score
        )
        {
			if(!course.IsEnrolled(reviewer))
			{
				throw new CourseReviewException("You must enroll this course."); 
			}
			return new Review()
			{
				Course = course,
				Reviewer = reviewer,
				Comment = comment,
				Score = score,
				CreateDateUTC = DateTime.UtcNow,
				LastUpdateDateUTC = DateTime.UtcNow
			};
        }

		public void Update(
			ApplicationUser reviewer, 
			string comment,
			double? score
		)
        {
			if(reviewer.Id != ReviewerId) 
			{
				throw new CourseReviewException("Only allow to update your own review");
			}
			Comment = comment ?? Comment;
			Score = score ?? Score;
			Likes = new List<Like>(); // remove all likes after modify the review 
			LastUpdateDateUTC = DateTime.UtcNow;
        }

		public void AddLike(ApplicationUser user) 
		{
			var like = Likes.SingleOrDefault(l => l.UserId == user.Id);
            if (like != null)
            {
				throw new CourseReviewException("You have liked this review already");
            }
			var newlike = Like.Create(user);
			Likes.Add(newlike);
		}

		public void RemoveLike(ApplicationUser user) 
		{
			// 1. check lecture 
			var like = Likes.SingleOrDefault(l => l.UserId == user.Id);
			if (like == null)
            {
                throw new NotFoundException("like not found");
            }
            // 2. remove like
			var likeList = Likes.ToList();
			likeList.Remove(like);
			Likes = likeList;
		}

    }
}
