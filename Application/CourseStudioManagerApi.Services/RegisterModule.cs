using Autofac;
using CourseStudioManager.Api.Services.CourseAttributes;
using CourseStudioManager.Api.Services.Courses;
using CourseStudioManager.Api.Services.Users;
using CourseStudioManager.Api.Services.Playlists;
using CourseStudioManager.Api.Services.Trades;

namespace CourseStudioManager.Api.Services
{
	public class RegisterModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<AdminUserService>().As<IAdminUserService>();
			builder.RegisterType<StudentService>().As<IStudentService>();
			builder.RegisterType<TutorService>().As<ITutorService>();
			builder.RegisterType<CourseService>().As<ICourseService>();
			builder.RegisterType<CourseAttributeService>().As<ICourseAttributeService>();
			builder.RegisterType<PlaylistService>().As<IPlaylistService>();
			builder.RegisterType<OrderService>().As<IOrderService>();
			builder.RegisterType<SaleService>().As<ISaleService>();
			builder.RegisterType<CouponService>().As<ICouponService>();

			builder.RegisterModule(new CourseStudio.Application.Dtos.RegisterModule());
			builder.RegisterModule(new CourseStudio.Application.Common.RegisterModule());
            builder.RegisterModule(new CourseStudio.Domain.Services.RegisterModule());
            builder.RegisterModule(new CourseStudio.Domain.Repositories.RegisterModule());
        }
    }
}
