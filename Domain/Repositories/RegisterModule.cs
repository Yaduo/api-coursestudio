using Autofac;
using CourseStudio.Domain.Repositories.CourseAttributes;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Identities;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Domain.Repositories.Playlists;
using CourseStudio.Domain.Repositories.Trades;

namespace CourseStudio.Domain.Repositories
{
    public class RegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
			builder.RegisterType<UserRepository>().As<IUserRepository>();
			builder.RegisterType<IdentityTokenRepository>().As<IIdentityTokenRepository>();
			builder.RegisterType<IdentityTokenBlacklistRepository>().As<IIdentityTokenBlacklistRepository>();
			builder.RegisterType<TutorRepository>().As<ITutorRepository>();
			builder.RegisterType<AdministratorRepository>().As<IAdministratorRepository>();
			builder.RegisterType<StudentRepository>().As<IStudentRepository>();
			builder.RegisterType<StudyRecordRepository>().As<IStudyRecordRepository>();

			builder.RegisterType<CourseRepository>().As<ICourseRepository>();
			builder.RegisterType<SectionRepository>().As<ISectionRepository>();
			builder.RegisterType<LectureRepository>().As<ILectureRepository>();
			builder.RegisterType<ContentRepository>().As<IContentRepository>();
			builder.RegisterType<VideoRepository>().As<IVideoRepository>();
            builder.RegisterType<CourseReviewRepository>().As<ICourseReviewRepository>();

            builder.RegisterType<CourseAttributesRepository>().As<ICourseAttributesRepository>();
            builder.RegisterType<EntityAttributeTypeRepository>().As<IEntityAttributeTypeRepository>();
			builder.RegisterType<EntityAttributeRepository>().As<IEntityAttributeRepository>();
            
			builder.RegisterType<CourseAuditingRepository>().As<ICourseAuditingRepository>();
            
			builder.RegisterType<PlaylistRepository>().As<IPlaylistRepository>();
            
			builder.RegisterType<ShoppingCartRepository>().As<IShoppingCartRepository>();
			builder.RegisterType<SalesOrderRepository>().As<ISalesOrderRepository>();
			builder.RegisterType<CouponRepository>().As<ICouponRepository>();

            builder.RegisterModule(new CourseStudio.Domain.Persistence.RegisterModule());
        }
    }
}
