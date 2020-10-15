using Autofac;
using CourseStudio.Api.Services.Users;
using CourseStudio.Api.Services.Courses;
using CourseStudio.Api.Services.Playlists;
using CourseStudio.Api.Services.Trades;
using CourseStudio.Api.Services.CourseAttributes;

namespace CourseStudio.Api.Services
{
    public class RegisterModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
			builder.RegisterType<UserServices>().As<IUserServices>();
			builder.RegisterType<TutorService>().As<ITutorService>();

			builder.RegisterType<CourseServices>().As<ICourseServices>();
			builder.RegisterType<SectionServices>().As<ISectionServices>();
			builder.RegisterType<LectureServices>().As<ILectureServices>();
			builder.RegisterType<ContentServices>().As<IContentServices>();
			builder.RegisterType<VideoServices>().As<IVideoServices>();
            builder.RegisterType<CourseReviewServices>().As<ICourseReviewServices>();
            builder.RegisterType<CourseAuditingServices>().As<ICourseAuditingServices>();
            
            builder.RegisterType<PlaylistServices>().As<IPlaylistServices>();
            builder.RegisterType<ShoppingCartServices>().As<IShoppingCartServices>();
			builder.RegisterType<SalesOrderServices>().As<ISalesOrderServices>();
			builder.RegisterType<CourseAttributeServices>().As<ICourseAttributeServices>();
			builder.RegisterType<CouponService>().As<ICouponService>();

			builder.RegisterModule(new CourseStudio.Application.Common.RegisterModule());
			builder.RegisterModule(new CourseStudio.Application.Dtos.RegisterModule());
            builder.RegisterModule(new CourseStudio.Domain.Services.RegisterModule());
            builder.RegisterModule(new CourseStudio.Domain.Repositories.RegisterModule());
        }
    }
}
