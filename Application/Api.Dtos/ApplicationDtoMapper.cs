using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using CourseStudio.Application.Dtos.Courses;
using CourseStudio.Application.Dtos.Playlists;
using CourseStudio.Application.Dtos.CourseAttributes;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Doamin.Models.Courses;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.Playlists;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Doamin.Models.Trades;
using CourseStudio.Doamin.Models.Pagination;
using CourseStudio.Lib.Utilities;

namespace CourseStudio.Application.Dtos
{
	public class ApplicationDtoMapper
	{
		private readonly RoleManager<IdentityRole> _roleRoleManager;

        public ApplicationDtoMapper(
			RoleManager<IdentityRole> roleRoleManager
		)
		{
			_roleRoleManager = roleRoleManager;
        }

		public void Init()
		{

			Mapper.Initialize(e =>
			{
				/*
                 * mapping from Modle to Dto
                 */
				// User
				MapToUserDto(e);
                MapToTutorDto(e);
                MapToTutorAuditingDto(e);
				MapToAdminDto(e);
				MapToPaymentProfileDto(e);
				MapToStudyRecordDto(e);

				// Course
				MapToCourseDto(e);
				MapToSectionDto(e);
				MapToLectureDto(e);
				MapToContentDto(e);
				MapToVideoDto(e);
                MapToDocumentDto(e);
                MapToPresentationDto(e);
                MapToLinkDto(e);
                MapToLikeDto(e);
                MapToCourseAttributeDto(e);
                MapToCourseReviewDto(e);
                MapToCourseAuditingDto(e);

                // Course Entity Attribute
                MapToEntityAttributeDto(e);
				MapToEntityAttributeTypeDto(e);

				// Playlist
				MapToPlaylistDto(e);

				// trade
				MapToShoppingCartDto(e);
				MapToSalesOrderDto(e);
				MapToLineItemDto(e);
				MapToTransactionRecordDto(e);

				// Coupons
				MapToCouponDto(e);
				MapToCouponRuleDto(e);
				MapToCouponScopeDto(e);

				// Pagination
				MapToPaginationDto<ApplicationUser, UserDto>(e);
				MapToPaginationDto<Tutor, TutorDto>(e);
				MapToPaginationDto<Administrator, AdminDto>(e);
				MapToPaginationDto<StudyRecord, StudyRecordDto>(e);

				MapToPaginationDto<Course, CourseDto>(e);
				MapToPaginationDto<EntityAttribute, EntityAttributeDto>(e);
				MapToPaginationDto<CourseAuditing, CourseAuditingDto>(e);
                MapToPaginationDto<Review, CourseReviewDto>(e);

                MapToPaginationDto<Order, SalesOrderDto>(e);
				MapToPaginationDto<Coupon, CouponDto>(e);
			});
		}

		/*
         * Mapping from Modle to Dto
         * */
		// User
		private void MapToUserDto(IMapperConfigurationExpression expression)
		{
			var roles = _roleRoleManager.Roles.ToList();
			expression.CreateMap<ApplicationUser, UserDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.AvatarUrl, opts => opts.MapFrom(s => s.AvatarUrl))
			.ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.FirstName))
			.ForMember(d => d.LastName, opts => opts.MapFrom(s => s.LastName))
			.ForMember(d => d.Email, opts => opts.MapFrom(s => s.Email))
			.ForMember(d => d.UserName, opts => opts.MapFrom(s => s.UserName))
			.ForMember(d => d.PhoneNumber, opts => opts.MapFrom(s => s.PhoneNumber))
			.ForMember(d => d.CreateDateUTC, opts => opts.MapFrom(s => s.CreateDateUTC))
			.ForMember(d => d.IsActivated, opts => opts.MapFrom(s => s.IsActivated))
			.ForMember(d => d.Roles, opts => opts.MapFrom(s => roles.Where(r => s.ApplicationUserRoles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name)))
			.ForMember(d => d.Tutor, opts => opts.MapFrom(s => Mapper.Map<TutorDto>(s.Tutor)))
			.ForMember(d => d.PaymentProfile, opts => opts.MapFrom(s => Mapper.Map<PaymentProfileDto>(s.PaymentProfile)))
			.ForAllOtherMembers(o => o.Ignore());
		}

		private void MapToTutorDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<Tutor, TutorDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(d => d.Avatar, opts => opts.MapFrom(src => src.ApplicationUser.AvatarUrl))
            .ForMember(d => d.UserName, opts => opts.MapFrom(s => s.ApplicationUser.UserName))
            .ForMember(d => d.FullName, opts => opts.MapFrom(s => s.ApplicationUser.FullName))
            .ForMember(d => d.Email, opts => opts.MapFrom(s => s.ApplicationUser.Email))
			.ForMember(d => d.CreateDateUTC, opts => opts.MapFrom(s => s.CreateDateUTC))
			.ForMember(d => d.IsActivated, opts => opts.MapFrom(s => s.IsActivated))
			.ForMember(d => d.Resume, opts => opts.MapFrom(s => s.Resume))
			.ForMember(d => d.TotalCoursesCount, opts => opts.MapFrom(s => s.TotalCoursesCount))
			.ForMember(d => d.TotalEnrollmentCount, opts => opts.MapFrom(s => s.TotalEnrollmentCount))
            .ForMember(d => d.CommissionRate, opts => opts.MapFrom(s => s.CommissionRate))
            .ForMember(d => d.AuditingRecords, opts => opts.MapFrom(s => Mapper.Map<IList<TutorAuditingDto>>(s.TutorAuditings)))
            .ForAllOtherMembers(o => o.Ignore());
		}

        private void MapToTutorAuditingDto(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<TutorAuditing, TutorAuditingDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(d => d.AuditorId, opts => opts.MapFrom(src => src.AuditorId))
            .ForMember(d => d.CreateDateUTC, opts => opts.MapFrom(src => src.CreateDateUTC))
            .ForMember(d => d.Note, opts => opts.MapFrom(src => src.Note))
            .ForMember(d => d.TutorId, opts => opts.MapFrom(src => src.TutorId))
            .ForMember(d => d.State, opts => opts.MapFrom(s => s.State.GetDescription()))
            .ForAllOtherMembers(o => o.Ignore());
        }

        private void MapToAdminDto(IMapperConfigurationExpression expression)
        {
			var roles = _roleRoleManager.Roles.ToList();
			expression.CreateMap<Administrator, AdminDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(d => d.Email, opts => opts.MapFrom(s => s.ApplicationUser.Email))
            .ForMember(d => d.CreateDateUTC, opts => opts.MapFrom(s => s.CreateDateUTC))
			.ForMember(d => d.IsActivated, opts => opts.MapFrom(s => s.IsActivated))
			.ForMember(d => d.Roles, opts => opts.MapFrom(s => roles.Where(r => s.ApplicationUser.ApplicationUserRoles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name)))
            .ForAllOtherMembers(o => o.Ignore());
        }

		private void MapToPaymentProfileDto(IMapperConfigurationExpression expression)
        {
			expression.CreateMap<PaymentProfile, PaymentProfileDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.CustomerCode, opts => opts.MapFrom(src => src.CustomerCode))
			.ForMember(dest => dest.PaymentToken, opts => opts.MapFrom(src => src.PaymentToken))
            .ForAllOtherMembers(o => o.Ignore());
        }
        
		private void MapToStudyRecordDto(IMapperConfigurationExpression expression)
        {
			expression.CreateMap<StudyRecord, StudyRecordDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.CourseId, opts => opts.MapFrom(s => s.CourseId))
			.ForMember(d => d.Course, opts => opts.MapFrom(s => Mapper.Map<CourseDto>(s.Course)))
			.ForMember(d => d.LastUpdateDateUTC, opts => opts.MapFrom(s => s.LastUpdateDateUTC))
			.ForMember(d => d.LectureIds, opts => opts.MapFrom(s => s.LectureIds))
            .ForAllOtherMembers(o => o.Ignore());
        }

		// Course
		private void MapToCourseDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<Course, CourseDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.Tutor, opts => opts.MapFrom(s => Mapper.Map<TutorDto>(s.Tutor)))
			.ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
			.ForMember(d => d.Subtitle, opts => opts.MapFrom(s => s.Subtitle))
			.ForMember(d => d.LanguageType, opts => opts.MapFrom(s => s.LanguageType.ToString()))
			.ForMember(d => d.CreateDateUTC, opts => opts.MapFrom(s => s.CreateDateUTC))
			.ForMember(d => d.Description, opts => opts.MapFrom(s => s.Description))
			.ForMember(d => d.TotalDurationInSeconds, opts => opts.MapFrom(s => s.TotalDurationInSeconds))
			.ForMember(d => d.LecturesCount, opts => opts.MapFrom(s => s.LecturesCount))
			.ForMember(d => d.Rating, opts => opts.MapFrom(s => s.Rating))
			.ForMember(d => d.RatesCount, opts => opts.MapFrom(s => s.RatesCount))
			.ForMember(d => d.EnrollmentCount, opts => opts.MapFrom(s => s.EnrollmentCount))
			.ForMember(d => d.Prerequisites, opts => opts.MapFrom(s => s.Prerequisites))
			.ForMember(d => d.Refferences, opts => opts.MapFrom(s => s.Refferences))
			.ForMember(d => d.TargetStudents, opts => opts.MapFrom(s => s.TargetStudents))
			.ForMember(d => d.IsReady, opts => opts.MapFrom(s => s.IsReady))
			.ForMember(d => d.IsActive, opts => opts.MapFrom(s => s.IsActivate))
			.ForMember(d => d.CoverPageImage, opts => opts.MapFrom(s => s.ImageUrl))
			.ForMember(d => d.State, opts => opts.MapFrom(s => s.State.GetDescription()))
			.ForMember(d => d.Sections, opts => opts.MapFrom(s => Mapper.Map<IList<SectionDto>>(s.Sections)))
			.ForMember(d => d.Attributes, opts => opts.MapFrom(s => Mapper.Map<IList<CourseAttributeDto>>(s.Attributes)))
            .ForMember(d => d.DiscountAmount, opts => opts.MapFrom(s => s.DiscountAmount))
			.ForMember(d => d.DiscountPercent, opts => opts.MapFrom(s => s.DiscountPercent))
			.ForMember(d => d.UnitPrice, opts => opts.MapFrom(s => s.UnitPrice))
			.ForMember(d => d.Price, opts => opts.MapFrom(s => s.Price))
            .ForMember(d => d.VimeoAlbumId, opts => opts.MapFrom(s => s.VimeoAlbumId))
            .ForMember(d => d.PreviewVideo, opts => opts.MapFrom(s => Mapper.Map<VideoDto>(s.PreviewVideo)))
            .ForAllOtherMembers(o => o.Ignore());
		}

		private void MapToSectionDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<Section, SectionDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.CourseId, opts => opts.MapFrom(src => src.CourseId))
			.ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
			.ForMember(d => d.SortOrder, opts => opts.MapFrom(s => s.SortOrder))
			.ForMember(d => d.Lectures, opts => opts.MapFrom(s => Mapper.Map<IList<LectureDto>>(s.Lectures)))
			.ForAllOtherMembers(o => o.Ignore());
		}

		private void MapToLectureDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<Lecture, LectureDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.SectionId, opts => opts.MapFrom(src => src.SectionId))
			.ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
			.ForMember(d => d.IsAllowPreview, opts => opts.MapFrom(s => s.IsAllowPreview))
			.ForMember(d => d.SortOrder, opts => opts.MapFrom(s => s.SortOrder))
			.ForMember(d => d.Contents, opts => opts.MapFrom(s => Mapper.Map<IList<ContentDto>>(s.Contents)))
			.ForAllOtherMembers(o => o.Ignore());
		}

		private void MapToContentDto(IMapperConfigurationExpression expression)
        {
			expression.CreateMap<Content, ContentDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.CreateDateUTC, opts => opts.MapFrom(s => s.CreateDateUTC))
			.ForMember(d => d.LastUpdateDateUTC, opts => opts.MapFrom(s => s.LastUpdateDateUTC))
			.ForMember(d => d.Type, opts => opts.MapFrom(s => (int)s.Type))
			.ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
			.ForMember(d => d.Desctiption, opts => opts.MapFrom(s => s.Desctiption))
			.ForMember(d => d.SortOrder, opts => opts.MapFrom(s => s.SortOrder))
			.ForMember(d => d.Video, opts => opts.MapFrom(s => Mapper.Map<VideoDto>(s.Video)))
            .ForAllOtherMembers(o => o.Ignore());
        }

		private void MapToVideoDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<Video, VideoDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.VimeoId, opts => opts.MapFrom(s => s.VimeoId))
			.ForMember(d => d.DurationInSecond, opts => opts.MapFrom(s => s.DurationInSecond))
			.ForAllOtherMembers(o => o.Ignore());
		}

		private void MapToLinkDto(IMapperConfigurationExpression expression)
        {
			expression.CreateMap<Link, LinkDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.Url, opts => opts.MapFrom(s => s.Url))
            .ForAllOtherMembers(o => o.Ignore());
        }

		private void MapToPresentationDto(IMapperConfigurationExpression expression)
        {
			expression.CreateMap<Presentation, PresentationDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.Url, opts => opts.MapFrom(s => s.Url))
            .ForAllOtherMembers(o => o.Ignore());
        }

		private void MapToDocumentDto(IMapperConfigurationExpression expression)
        {
			expression.CreateMap<Document, DocumentDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.Url, opts => opts.MapFrom(s => s.Url))
            .ForAllOtherMembers(o => o.Ignore());
        }

		private void MapToCourseAttributeDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<CourseAttribute, CourseAttributeDto>()
			.ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
			.ForMember(d => d.AttributeId, opts => opts.MapFrom(s => s.EntityAttribute.Id))
			.ForMember(d => d.TypeId, opts => opts.MapFrom(s => s.EntityAttribute.EntityAttributeType.Id))
			.ForMember(d => d.Type, opts => opts.MapFrom(s => s.EntityAttribute.EntityAttributeType.Name))
			.ForMember(d => d.Value, opts => opts.MapFrom(s => s.EntityAttribute.Value))
			.ForMember(d => d.ParentId, opts => opts.MapFrom(s => s.EntityAttribute.ParentId))
			.ForAllOtherMembers(o => o.Ignore());
		}

		// Course Entity Attribute
		private void MapToEntityAttributeDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<EntityAttribute, EntityAttributeDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.EntityAttributeTypeId, opts => opts.MapFrom(src => src.EntityAttributeTypeId))
			.ForMember(dest => dest.Value, opts => opts.MapFrom(src => src.Value))
			.ForMember(dest => dest.ParentId, opts => opts.MapFrom(src => src.ParentId))
			.ForMember(dest => dest.IsSearchable, opts => opts.MapFrom(src => src.IsSearchable))
			.ForMember(dest => dest.Courses, opts => opts.MapFrom(src => Mapper.Map<IList<CourseDto>>(src.CourseAttributes.Select(ca => ca.Course).ToList())))
			.ForAllOtherMembers(o => o.Ignore());
		}

		private void MapToEntityAttributeTypeDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<EntityAttributeType, EntityAttributeTypeDto>()
				.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
				.ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
				.ForMember(d => d.EntityAttributes, opts => opts.MapFrom(s => Mapper.Map<IList<EntityAttributeDto>>(s.EntityAttributes)))
				.ForAllOtherMembers(o => o.Ignore());
		}

        private void MapToCourseReviewDto(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<Review, CourseReviewDto>() 
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(d => d.CourseId, opts => opts.MapFrom(s => s.CourseId))
			.ForMember(d => d.ReviewerId, opts => opts.MapFrom(s => s.ReviewerId))
			.ForMember(d => d.Comment, opts => opts.MapFrom(s => s.Comment))
			.ForMember(d => d.Score, opts => opts.MapFrom(s => s.Score))
            .ForMember(d => d.CreateDateUTC, opts => opts.MapFrom(s => s.CreateDateUTC))
            .ForMember(d => d.LastUpdateDateUTC, opts => opts.MapFrom(s => s.LastUpdateDateUTC))
			.ForMember(d => d.Reviewer, opts => opts.MapFrom(s => Mapper.Map<UserDto>(s.Reviewer)))
			.ForMember(d => d.Likes, opts => opts.MapFrom(s => Mapper.Map<IList<LikeDto>>(s.Likes)))
            .ForAllOtherMembers(o => o.Ignore());
        }

		private void MapToLikeDto(IMapperConfigurationExpression expression)
        {
			expression.CreateMap<Like, LikeDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.ReviewId, opts => opts.MapFrom(s => s.ReviewId))
			.ForMember(d => d.UserId, opts => opts.MapFrom(s => s.UserId))
			.ForMember(d => d.CreateDateUTC, opts => opts.MapFrom(s => s.CreateDateUTC))
            .ForAllOtherMembers(o => o.Ignore());
        }
      
        // Playlist
        private void MapToPlaylistDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<Playlist, PlaylistDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.PlaylistsType, opts => opts.MapFrom(src => src.PlaylistsType))
			.ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
			.ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
			.ForMember(dest => dest.IsPublic, opts => opts.MapFrom(src => src.IsPublic))
			.ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.User.Id))
			.ForMember(d => d.Courses, opts => opts.MapFrom(s => s.PlaylistCourses.Select(pc => Mapper.Map<CourseDto>(pc.Course)).ToList()))
			.ForAllOtherMembers(o => o.Ignore());
		}

		//Sales Order
		private void MapToShoppingCartDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<ShoppingCart, ShoppingCartDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.UserId, opts => opts.MapFrom(s => s.UserId))
			.ForMember(d => d.DiscountPercent, opts => opts.MapFrom(s => s.DiscountPercent))
            .ForMember(d => d.DiscountAmount, opts => opts.MapFrom(s => s.DiscountAmount))
            .ForMember(d => d.AmountOriginal, opts => opts.MapFrom(s => s.AmountOriginal))
            .ForMember(d => d.AmountTotal, opts => opts.MapFrom(s => s.AmountTotal))
			.ForMember(d => d.ShoppingCartItems, opts => opts.MapFrom(s => Mapper.Map<IList<LineItemDto>>(s.ShoppingCartItems)))
			.ForMember(d => d.ShoppingCartCoupons, opts => opts.MapFrom(s => Mapper.Map<IList<CouponDto>>(s.ShoppingCartCoupons.Select(sc => sc.Coupon))))
			.ForAllOtherMembers(o => o.Ignore());
		}

		private void MapToSalesOrderDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<Order, SalesOrderDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.OrderNumber, opts => opts.MapFrom(s => s.OrderNumber))
			.ForMember(d => d.UserId, opts => opts.MapFrom(s => s.UserId))
			.ForMember(d => d.State, opts => opts.MapFrom(s => s.State.GetDescription()))
			.ForMember(d => d.CreateDateUTC, opts => opts.MapFrom(s => s.CreateDateUTC))
			.ForMember(d => d.DiscountPercent, opts => opts.MapFrom(s => s.DiscountPercent))
			.ForMember(d => d.DiscountAmount, opts => opts.MapFrom(s => s.DiscountAmount))
			.ForMember(d => d.AmountOriginal, opts => opts.MapFrom(s => s.AmountOriginal))
			.ForMember(d => d.AmountTotal, opts => opts.MapFrom(s => s.AmountTotal))
			.ForMember(d => d.OrderItems, opts => opts.MapFrom(s => Mapper.Map<IList<LineItemDto>>(s.OrderItems)))
			.ForMember(d => d.TransactionRecords, opts => opts.MapFrom(s => Mapper.Map<IList<TransactionRecordDto>>(s.TransactionRecords)))
			.ForMember(d => d.Coupons, opts => opts.MapFrom(s => Mapper.Map<IList<CouponDto>>(s.OrderCoupons.Select(or => or.Coupon))))
			.ForAllOtherMembers(o => o.Ignore());
		}

		private void MapToLineItemDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<LineItem, LineItemDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.OrderId, opts => opts.MapFrom(s => s.OrderId))
			.ForMember(d => d.ShoppingCartId, opts => opts.MapFrom(s => s.ShoppingCartId))
			.ForMember(d => d.CourseId, opts => opts.MapFrom(s => s.CourseId))
            .ForMember(d => d.Course, opts => opts.MapFrom(s => Mapper.Map<CourseDto>(s.Course)))
            .ForMember(d => d.Quantity, opts => opts.MapFrom(s => s.Quantity))
            .ForMember(d => d.UnitPrice, opts => opts.MapFrom(s => s.UnitPrice))
            .ForMember(d => d.DiscountPercent, opts => opts.MapFrom(s => s.DiscountPercent))
            .ForMember(d => d.DiscountAmount, opts => opts.MapFrom(s => s.DiscountAmount))
            .ForMember(d => d.PriceOriginal, opts => opts.MapFrom(s => s.PriceOriginal))
            .ForMember(d => d.Price, opts => opts.MapFrom(s => s.Price))
            .ForMember(d => d.IsDiscounted, opts => opts.MapFrom(s => s.IsDiscounted))
            .ForAllOtherMembers(o => o.Ignore());
		}

		private void MapToTransactionRecordDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<TransactionRecord, TransactionRecordDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.OrderId, opts => opts.MapFrom(src => src.OrderId))
			.ForMember(dest => dest.Metadata, opts => opts.MapFrom(src => src.Metadata))
			.ForMember(dest => dest.Status, opts => opts.MapFrom(src => src.Status.GetDescription()))
			.ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Type.GetDescription()))
			.ForMember(dest => dest.CreateDateUtc, opts => opts.MapFrom(src => src.CreateDateUTC))
			.ForAllOtherMembers(o => o.Ignore());
		}

		// Course Auditing
		private void MapToCourseAuditingDto(IMapperConfigurationExpression expression)
		{
			expression.CreateMap<CourseAuditing, CourseAuditingDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(d => d.CourseId, opts => opts.MapFrom(s => s.CourseId))
            .ForMember(d => d.Course, opts => opts.MapFrom(s => Mapper.Map<CourseDto>(s.Course)))
            .ForMember(d => d.AuditorId, opts => opts.MapFrom(s => s.AuditorId))
            .ForMember(d => d.Auditor, opts => opts.MapFrom(s => Mapper.Map<AdminDto>(s.Auditor)))
            .ForMember(d => d.CreateDateUTC, opts => opts.MapFrom(s => s.CreateDateUTC))
            .ForMember(d => d.Note, opts => opts.MapFrom(s => s.Note))
            .ForMember(d => d.State, opts => opts.MapFrom(s => s.State.GetDescription()))
			.ForAllOtherMembers(o => o.Ignore());
		}

        // Coupons
		private void MapToCouponDto(IMapperConfigurationExpression expression)
		{ 
			expression.CreateMap<Coupon, CouponDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(d => d.Code, opts => opts.MapFrom(s => s.Code))
			.ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
			.ForMember(d => d.Description, opts => opts.MapFrom(s => s.Description))
			.ForMember(d => d.IsActivate, opts => opts.MapFrom(s => s.IsActivate))
			.ForMember(d => d.CreatDateUTC, opts => opts.MapFrom(s => s.CreatDateUTC))
			.ForMember(d => d.StartTimeUTC, opts => opts.MapFrom(s => s.StartTimeUTC))
			.ForMember(d => d.EndTimeUTC, opts => opts.MapFrom(s => s.EndTimeUTC))
			.ForMember(d => d.CouponRules, opts => opts.MapFrom(s => Mapper.Map<IList<CouponRuleDto>>(s.CouponRules)))
			.ForMember(d => d.Scopes, opts => opts.MapFrom(s => Mapper.Map<IList<ScopeDto>>(s.Scopes)))
            .ForAllOtherMembers(o => o.Ignore());
		}

		private void MapToCouponRuleDto(IMapperConfigurationExpression expression)
        {
			expression.CreateMap<CouponRule, CouponRuleDto>()
			.ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.CouponId, opts => opts.MapFrom(src => src.CouponId))
			.ForMember(dest => dest.MemberName, opts => opts.MapFrom(src => src.MemberName))
			.ForMember(d => d.Operator, opts => opts.MapFrom(s => s.Operator))
			.ForMember(d => d.TargetValue, opts => opts.MapFrom(s => s.TargetValue))
            .ForAllOtherMembers(o => o.Ignore());
        }

		private void MapToCouponScopeDto(IMapperConfigurationExpression expression)
        {
			expression.CreateMap<Scope, ScopeDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
			.ForMember(dest => dest.CouponId, opts => opts.MapFrom(src => src.CouponId))
			.ForMember(dest => dest.Level, opts => opts.MapFrom(src => src.Level.GetDescription()))
			.ForMember(dest => dest.CourseId, opts => opts.MapFrom(src => src.CourseId))
			.ForMember(dest => dest.DiscountAmount, opts => opts.MapFrom(src => src.DiscountAmount))
			.ForMember(dest => dest.DiscountPercent, opts => opts.MapFrom(src => src.DiscountPercent))
			.ForMember(dest => dest.Quantity, opts => opts.MapFrom(src => src.Quantity))
            .ForAllOtherMembers(o => o.Ignore());
        }

        // Pagination
		private void MapToPaginationDto<T, TDto>(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<PagedList<T>, PaginationDto<TDto>>()
            .ForMember(dest => dest.Items, opts => opts.MapFrom(src => Mapper.Map<IList<TDto>>(src)))
			.ForMember(d => d.CurrentPage, opts => opts.MapFrom(s => s.CurrentPage))
			.ForMember(d => d.PageSize, opts => opts.MapFrom(s => s.PageSize))
			.ForMember(d => d.TotalCount, opts => opts.MapFrom(s => s.TotalCount))
			.ForMember(d => d.TotalPages, opts => opts.MapFrom(s => s.TotalPages))
            .ForAllOtherMembers(o => o.Ignore());
        }
	}
}

